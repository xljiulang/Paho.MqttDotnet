using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 追踪级别
    /// </summary>
    public enum MqttTraceLevels
    {
        /// <summary>
        /// 最多消息
        /// </summary>
        Maximum = 1,

        /// <summary>
        /// 中等消息
        /// </summary>
        Medium,

        /// <summary>
        /// 最小消息
        /// </summary>
        Minimum,

        /// <summary>
        /// 协议
        /// </summary>
        Protocol,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 苛刻
        /// </summary>
        Severe,

        /// <summary>
        /// 致命
        /// </summary>
        Fatal,
    }
}
