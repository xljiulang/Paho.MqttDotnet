using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Runtime.InteropServices;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 断开丢失委托
    /// </summary>
    /// <param name="sender">触发者</param>
    public delegate void ConnectionLostdHandler(object sender);

    /// <summary>
    /// 收到消息委托
    /// </summary>
    /// <param name="sender">触发者</param>
    /// <param name="topic">主题</param>
    /// <param name="message">消息</param>
    public delegate void MessageArrivedHandler(object sender, string topic, MqttMessage message);

    /// <summary>
    /// 表示mqtt客户端
    /// </summary>
    unsafe public class MqttClient : SafeHandle
    {
        /// <summary>
        /// 用于保存最近的连接配置项
        /// </summary>
        private ConnectOption connectOpt;

        /// <summary>
        /// 用于生成任务的id
        /// </summary>
        private int taskIdValue = 0;

        /// <summary>
        /// 用于保存对象的引用
        /// </summary>
        private readonly HashSet<object> hashSet = new HashSet<object>();

        /// <summary>
        /// 用于保存当前正在执行的任务
        /// </summary>
        private readonly TaskSetterTable<int> taskSetterTable = new TaskSetterTable<int>();


        /// <summary>
        /// 连接断开事件
        /// </summary>
        public event ConnectionLostdHandler OnConnectionLost;

        /// <summary>
        /// 收到消息事件
        /// </summary>
        public event MessageArrivedHandler OnMessageArrived;

        /// <summary>
        /// 获取是否已连接
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return MQTTAsync.MQTTAsync_isConnected(this.handle) > 0;
            }
        }

        /// <summary>
        /// 获取句柄是否无效
        /// </summary>
        public sealed override bool IsInvalid
        {
            get
            {
                return this.handle == IntPtr.Zero;
            }
        }

        /// <summary>
        /// mqtt客户端
        /// </summary>
        /// <param name="serverUri">mqtt://mymqtt.com</param>
        /// <param name="clientId">客户端id</param>
        /// <exception cref="ArgumentNullException"></exception>    
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="MqttException"></exception>
        public MqttClient(string serverUri, string clientId) :
            this(serverUri, clientId, MqttPersistence.None)
        {
        }

        /// <summary>
        /// mqtt客户端
        /// </summary>
        /// <param name="serverUri">mqtt://mymqtt.com</param>
        /// <param name="clientId">客户端id</param>
        /// <param name="persistence">持久化方式</param>
        /// <exception cref="ArgumentNullException"></exception>    
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="MqttException"></exception>
        public MqttClient(string serverUri, string clientId, MqttPersistence persistence)
            : base(IntPtr.Zero, true)
        {
            if (string.IsNullOrEmpty(serverUri))
            {
                throw new ArgumentNullException("serverUri");
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (persistence == MqttPersistence.User)
            {
                var message = string.Format("不支持的持久化方式：{0}.{1}", typeof(MqttPersistence).Name, persistence);
                throw new NotSupportedException(message);
            }

            var uri = new Uri(serverUri);
            const int mqttDefaultPort = 1883;
            var tcpUri = string.Format("tcp://{0}:{1}", uri.Host, uri.Port > 0 ? uri.Port : mqttDefaultPort);

            var er = MQTTAsync.MQTTAsync_create(ref this.handle, tcpUri, clientId, persistence, IntPtr.Zero);
            this.EnsureSuccessCode(er);

            this.InitClientCallbacks();
        }

        /// <summary>
        /// 设置客户端相关回调
        /// </summary>
        private void InitClientCallbacks()
        {
            var lost = new MQTTAsync_connectionLost((context, cause) =>
            {
                var e = this.OnConnectionLost;
                if (e != null)
                {
                    e.Invoke(this);
                }
            });

            var arrvied = new MQTTAsync_messageArrived((context, topicName, topicLen, msg) =>
            {
                var e = this.OnMessageArrived;
                if (e != null)
                {
                    var topic = Marshal.PtrToStringAnsi(topicName, topicLen);
                    var message = MqttMessage.From(msg);
                    e.Invoke(this, topic, message);
                }

                MQTTAsync.MQTTAsync_free(topicName);
                MQTTAsync.MQTTAsync_freeMessage(ref msg);
                return 1;
            });

            var er = MQTTAsync.MQTTAsync_setCallbacks(this.handle, IntPtr.Zero, lost, arrvied, null);
            this.EnsureSuccessCode(er);
            this.AutoRef(lost, arrvied);
        }


        /// <summary>
        /// 确保mqtt正确
        /// </summary>
        /// <param name="er">客户端错误码</param>
        /// <exception cref="MqttException"></exception>
        private void EnsureSuccessCode(MqttError er)
        {
            if (er != MqttError.Success)
            {
                throw new MqttException(er);
            }
        }

        /// <summary>
        /// 添加委托引用
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private void AutoRef(params Delegate[] d)
        {
            foreach (var item in d)
            {
                this.hashSet.Add(item);
            }
        }

        /// <summary>
        /// 创建并维护IMqttOptions引用
        /// </summary>
        /// <typeparam name="TMqttOptions"></typeparam>
        /// <returns></returns>
        private TMqttOptions AutoRef<TMqttOptions>() where TMqttOptions : IMqttOptions, new()
        {
            var opt = new TMqttOptions();
            opt.OnCompleted((taskId, value) =>
            {
                this.hashSet.Remove(opt);
                this.taskSetterTable.Remove(taskId).SetResult(value);
            });

            opt.OnException((taskId, ex) =>
            {
                this.hashSet.Remove(opt);
                this.taskSetterTable.Remove(taskId).SetException(ex);
            });

            this.hashSet.Add(opt);
            return opt;
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="option">选项</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public ConnectError Connect(ConnectOption option)
        {
            return this.ConnectInternal(option).GetResult();
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="option">选项</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public Task<ConnectError> ConnectAsync(ConnectOption option)
        {
            return this.ConnectInternal(option).GetTask();
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="option">选项</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        private ITaskSetter<ConnectError> ConnectInternal(ConnectOption option)
        {
            if (option == null)
            {
                throw new ArgumentNullException();
            }

            this.connectOpt = option;

            var taskId = this.GenerateTaskId();
            var setter = this.taskSetterTable.Create<ConnectError>(taskId);

            var mqttOptions = this.AutoRef<MqttConnectOptions>();
            var opt = mqttOptions.ToStruct(taskId, option.ToStruct());
            var er = MQTTAsync.MQTTAsync_connect(this.handle, ref opt);
            this.EnsureSuccessCode(er);

            return setter;
        }


        /// <summary>
        /// 生成任务id
        /// </summary>
        /// <returns></returns>
        private int GenerateTaskId()
        {
            return Interlocked.Increment(ref this.taskIdValue);
        }

        /// <summary>
        /// 重连到服务端
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public ConnectError ReConnect()
        {
            if (this.connectOpt == null)
            {
                throw new NotSupportedException();
            }
            return this.Connect(this.connectOpt);
        }

        /// <summary>
        /// 重连到服务端
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public Task<ConnectError> ReConnectAsync()
        {
            if (this.connectOpt == null)
            {
                throw new NotSupportedException();
            }
            return this.ConnectAsync(this.connectOpt);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public bool Disconnect()
        {
            return this.DisconnectInternal().GetResult();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public Task<bool> DisconnectAsync()
        {
            return this.DisconnectInternal().GetTask();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        private ITaskSetter<bool> DisconnectInternal()
        {
            var taskId = this.GenerateTaskId();
            var setter = this.taskSetterTable.Create<bool>(taskId);

            var mqttOptions = this.AutoRef<MqttDisconnecOptions>();
            var opt = mqttOptions.ToStruct(taskId);
            var er = MQTTAsync.MQTTAsync_disconnect(this.handle, ref opt);
            this.EnsureSuccessCode(er);

            return setter;
        }



        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="qos">质量</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public bool Subscribe(string topic, MqttQoS qos)
        {
            return this.SubscribeInternal(topic, qos).GetResult();
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="qos">质量</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public Task<bool> SubscribeAsync(string topic, MqttQoS qos)
        {
            return this.SubscribeInternal(topic, qos).GetTask();
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="qos">质量</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        private ITaskSetter<bool> SubscribeInternal(string topic, MqttQoS qos)
        {
            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentNullException("topic");
            }

            var taskId = this.GenerateTaskId();
            var setter = this.taskSetterTable.Create<bool>(taskId);

            var opt = this.InitResponseOptions(taskId);
            var er = MQTTAsync.MQTTAsync_subscribe(this.handle, topic, qos, ref opt);
            this.EnsureSuccessCode(er);

            return setter;
        }

        /// <summary>
        /// 订阅多个主题
        /// </summary>
        /// <param name="topicQos">主题与消息</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public bool SubscribeMany(params MqttTopicQoS[] topicQos)
        {
            return this.SubscribeManyInternal(topicQos).GetResult();
        }

        /// <summary>
        /// 订阅多个主题
        /// </summary>
        /// <param name="topicQos">主题与消息</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public Task<bool> SubscribeManyAsync(params MqttTopicQoS[] topicQos)
        {
            return this.SubscribeManyInternal(topicQos).GetTask();
        }


        /// <summary>
        /// 订阅多个主题
        /// </summary>
        /// <param name="topicQos">主题与消息</param>
        /// <param name="taskId">任务id</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        private ITaskSetter<bool> SubscribeManyInternal(MqttTopicQoS[] topicQos)
        {
            if (topicQos == null)
            {
                throw new ArgumentNullException();
            }
            if (topicQos.Length == 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var taskId = this.GenerateTaskId();
            var setter = this.taskSetterTable.Create<bool>(taskId);

            var opt = this.InitResponseOptions(taskId);
            var qoss = topicQos.Select(item => (int)item.QoS).ToArray();
            var topics = topicQos.Select(item => item.Topic.ToUnmanagedPointer()).ToArray();

            fixed (IntPtr* ptrtopic = &topics[0])
            {
                fixed (int* ptrqos = &qoss[0])
                {
                    var er = MQTTAsync.MQTTAsync_subscribeMany(this.handle, qoss.Length, ptrtopic, ptrqos, ref opt);
                    this.EnsureSuccessCode(er);
                }
            }

            return setter;
        }


        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="topic">主题</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public bool Unsubscribe(string topic)
        {
            return this.UnsubscribeInternal(topic).GetResult();
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="topic">主题</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public Task<bool> UnsubscribeAsync(string topic)
        {
            return this.UnsubscribeInternal(topic).GetTask();
        }


        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="topic">主题</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        private ITaskSetter<bool> UnsubscribeInternal(string topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException();
            }

            var taskId = this.GenerateTaskId();
            var setter = this.taskSetterTable.Create<bool>(taskId);

            var opt = this.InitResponseOptions(taskId);
            var er = MQTTAsync.MQTTAsync_unsubscribe(this.handle, topic, ref opt);
            this.EnsureSuccessCode(er);

            return setter;
        }


        /// <summary>
        /// 取消订阅多个主题
        /// </summary>
        /// <param name="topic">主题</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public bool UnsubscribeMany(params string[] topic)
        {
            return this.UnsubscribeManyInternal(topic).GetResult();
        }

        /// <summary>
        /// 取消订阅多个主题
        /// </summary>
        /// <param name="topic">主题</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public Task<bool> UnsubscribeManyAsync(params string[] topic)
        {
            return this.UnsubscribeManyInternal(topic).GetTask();
        }

        /// <summary>
        /// 取消订阅多个主题
        /// </summary>
        /// <param name="topic">主题</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        private ITaskSetter<bool> UnsubscribeManyInternal(string[] topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException();
            }
            if (topic.Length == 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var taskId = this.GenerateTaskId();
            var setter = this.taskSetterTable.Create<bool>(taskId);

            var topicArray = topic.Select(item => item.ToUnmanagedPointer()).ToArray();
            fixed (IntPtr* ptrtopic = &topicArray[0])
            {
                var opt = this.InitResponseOptions(taskId);
                var er = MQTTAsync.MQTTAsync_unsubscribeMany(this.handle, topic.Length, ptrtopic, ref opt);
                this.EnsureSuccessCode(er);
            }

            return setter;
        }


        /// <summary>
        /// 发送消息
        /// 不跟踪服务端的消息处理结果
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="message">消息</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public MqttError SendMessageAsync(string topic, MqttMessage message)
        {
            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentNullException("topic");
            }
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var msg = message.ToStruct();
            var opt = new MQTTAsync_responseOptions();
            opt.Init();

            var er = MQTTAsync.MQTTAsync_sendMessage(this.handle, topic, ref msg, ref opt);
            msg.Dispose();
            return er;
        }


        /// <summary>
        /// 发送消息
        /// 并跟踪服务端对消息回复结果
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="message">消息</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MqttException"></exception>
        /// <returns></returns>
        public Task<bool> SendMessageTaskAsync(string topic, MqttMessage message)
        {
            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentNullException("topic");
            }
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var taskId = this.GenerateTaskId();
            var setter = this.taskSetterTable.Create<bool>(taskId);

            var msg = message.ToStruct();
            var opt = this.InitResponseOptions(taskId);
            var er = MQTTAsync.MQTTAsync_sendMessage(this.handle, topic, ref msg, ref opt);
            msg.Dispose();

            this.EnsureSuccessCode(er);
            return setter.GetTask();
        }

        /// <summary>
        /// 初始化回复选项
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <returns></returns>
        private MQTTAsync_responseOptions InitResponseOptions(int taskId)
        {
            var mqttOptions = this.AutoRef<MqttResponseOptions>();
            var opt = mqttOptions.ToStruct(taskId);
            return opt;
        }

        /// <summary>
        /// 释放句柄
        /// </summary>
        protected sealed override bool ReleaseHandle()
        {
            if (this.IsInvalid == true)
            {
                return false;
            }

            MQTTAsync.MQTTAsync_destroy(ref this.handle);
            return this.IsInvalid;
        }


        /// <summary>
        /// 追踪回调
        /// </summary>
        private static MQTTAsync_traceCallback trackCallback;

        /// <summary>
        /// 设置追踪级别
        /// </summary>
        /// <param name="level">级别</param>
        public static void SetTraceLevel(MqttTraceLevels level)
        {
            MQTTAsync.MQTTAsync_setTraceLevel(level);
        }

        /// <summary>
        /// 设置追踪回调
        /// </summary>
        /// <param name="traceCallback">追踪回调，null则清除追踪</param>
        public static void SetTraceCallback(Action<MqttTraceLevels, string> traceCallback)
        {
            if (traceCallback == null)
            {
                MQTTAsync.MQTTAsync_setTraceCallback(null);
            }
            else
            {
                MqttClient.trackCallback = new MQTTAsync_traceCallback((level, msg) => traceCallback(level, msg));
                MQTTAsync.MQTTAsync_setTraceCallback(MqttClient.trackCallback);
            }
        }
    }
}
