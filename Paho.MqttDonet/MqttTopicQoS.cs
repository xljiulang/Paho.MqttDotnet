using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示主题和质量
    /// </summary>
    public class MqttTopicQoS
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 质量
        /// </summary>
        public MqttQoS QoS { get; set; }
    }
}
