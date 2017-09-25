using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示mqtt连接选项
    /// </summary>
    public class ConnectOption
    {
        /// <summary>
        /// 获取或设置心跳检测时间隔(s)
        /// </summary>
        public int KeepAliveInterval { get; set; }

        /// <summary>
        /// 获取或设置是否清除会话
        /// </summary>
        public bool CleanSession { get; set; }

        /// <summary>
        /// 获取或设置消息最大飞行窗口
        /// </summary>
        public int MaxInflight { get; set; }

        /// <summary>
        /// 获取或设置遗属
        /// </summary>
        public MqttWill Will { get; set; }

        /// <summary>
        /// 获取或设置账号
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置连接超时时间(s)
        /// </summary>
        public int ConnectTimeout { get; set; }

        /// <summary>
        /// 获取或设置超时重连的时间间隔(s)
        /// </summary>
        public int RetryInterval { get; set; }

        /// <summary>
        /// 获取或设置当断开后是否自动重连
        /// </summary>
        public bool AutomaticReconnect { get; set; }

        /// <summary>
        /// 获取或设置最小重连时间间隔(s)
        /// </summary>
        public int MinRetryInterval { get; set; }

        /// <summary>
        /// 获取或设置最大重连时间间隔(s)
        /// </summary>
        public int MaxRetryInterval { get; set; }

        /// <summary>
        /// mqtt连接选项
        /// </summary>
        public ConnectOption()
        {
            this.CleanSession = true;
            this.KeepAliveInterval = 60;
            this.MaxInflight = 10;
            this.ConnectTimeout = 30;
            this.MinRetryInterval = 1;
            this.MaxRetryInterval = 60;
        }

        /// <summary>
        /// 转换为结构体
        /// </summary>
        /// <returns></returns>
        internal MQTTAsync_connectOptions ToStruct()
        {
            var opt = new MQTTAsync_connectOptions();
            opt.Init();

            opt.keepAliveInterval = this.KeepAliveInterval;
            opt.cleansession = this.CleanSession ? 1 : 0;
            opt.maxInflight = this.MaxInflight;
            opt.username = this.Username.ToUnmanagedPointer();
            opt.password = this.Password.ToUnmanagedPointer();
            opt.connectTimeout = this.ConnectTimeout;

            opt.retryInterval = this.RetryInterval;
            opt.automaticReconnect = this.AutomaticReconnect ? 1 : 0;
            opt.minRetryInterval = this.MinRetryInterval;
            opt.maxRetryInterval = this.MaxRetryInterval;

            if (this.Will != null)
            {
                var will = this.Will.ToStruct();
                var willPtr = Marshal.AllocHGlobal(Marshal.SizeOf(will));
                Marshal.StructureToPtr(will, willPtr, true);
                opt.will = willPtr;
            }
            return opt;
        }
    }
}
