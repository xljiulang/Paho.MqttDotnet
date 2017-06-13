using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示mqtt遗属
    /// </summary>
    public class MqttWill
    {
        /// <summary>
        /// 获取或设置主题
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 获取或设置消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置是否存储遗属
        /// </summary>
        public bool Retain { get; set; }

        /// <summary>
        /// 获取或设置消息质量等级
        /// </summary>
        public MqttQoS Qos { get; set; }

        /// <summary>
        /// 转换为结构体
        /// </summary>
        /// <returns></returns>
        internal MQTTAsync_willOptions ToStruct()
        {
            var will = MQTTAsync_willOptions.Init();
            will.topicName = this.Topic.ToUnmanagedPointer();
            will.message = this.Message.ToUnmanagedPointer();
            will.retained = this.Retain ? 1 : 0;
            will.qos = (int)this.Qos;
            return will;
        }
    }
}
