using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示mqtt消息
    /// </summary>
    public class MqttMessage
    {
        /// <summary>
        /// 获取或设置是否为重发的消息
        /// </summary>
        public bool Dup { get; set; }

        /// <summary>
        /// 获取或设置消息质量等级
        /// </summary>
        public MqttQoS QoS { get; set; }

        /// <summary>
        /// 获取或设置是否存储消息
        /// </summary>
        public bool Retain { get; set; }

        /// <summary>
        /// 获取或设置有效数据
        /// </summary>
        public byte[] Payload { get; set; }

        /// <summary>
        /// mqtt消息
        /// </summary>
        public MqttMessage()
        {
        }

        /// <summary>
        /// mqtt消息
        /// </summary>
        /// <param name="qos">qos</param>
        /// <param name="payload">数据</param>
        public MqttMessage(MqttQoS qos, string payload)
        {
            this.QoS = qos;
            this.Payload = Encoding.UTF8.GetBytes(payload);
        }

        /// <summary>
        /// mqtt消息
        /// </summary>
        /// <param name="qos">qos</param>
        /// <param name="payload">数据</param>
        public MqttMessage(MqttQoS qos, byte[] payload)
        {
            this.QoS = qos;
            this.Payload = payload;
        }

        /// <summary>
        /// 将消息有效数据转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Encoding.UTF8.GetString(this.Payload);
        }

        /// <summary>
        /// 从message指针转换
        /// </summary>
        /// <param name="ptrmessage">MQTTAsync_message的指针</param>
        /// <returns></returns>
        internal static MqttMessage From(IntPtr ptrmessage)
        {
            var msg = (MQTTAsync_message)Marshal.PtrToStructure(ptrmessage, typeof(MQTTAsync_message));
            var message = new MqttMessage
            {
                Dup = msg.dup > 0,
                QoS = (MqttQoS)msg.qos,
                Retain = msg.retained > 0
            };

            message.Payload = new byte[msg.payloadlen];
            Marshal.Copy(msg.payload, message.Payload, 0, msg.payloadlen);
            return message;
        }

        /// <summary>
        /// 转换为结构体
        /// </summary>
        /// <returns></returns>
        internal MQTTAsync_message ToStruct()
        {
            if (this.Payload == null)
            {
                this.Payload = new byte[0];
            }

            var msg = MQTTAsync_message.Init();
            msg.retained = this.Retain ? 1 : 0;
            msg.payloadlen = this.Payload.Length;
            msg.qos = (int)this.QoS;
            msg.payload = Marshal.AllocHGlobal(this.Payload.Length);
            Marshal.Copy(this.Payload, 0, msg.payload, this.Payload.Length);
            return msg;
        }
    }
}
