using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示mqtt消息质量等级
    /// </summary>
    public enum MqttQoS : byte
    {
        /// <summary>
        /// 最多分发一次
        /// </summary>
        AtMostOnce = 0,

        /// <summary>
        /// 至少分发一次
        /// </summary>
        AtLeastOnce = 1,

        /// <summary>
        /// 仅分发一次
        /// </summary>
        ExactlyOnce = 2,
    }
}
