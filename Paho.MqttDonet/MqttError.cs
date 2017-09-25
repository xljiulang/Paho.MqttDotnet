using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示mqtt客户端错误码
    /// </summary>
    public enum MqttError
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Failure = -1,

        /// <summary>
        /// 持久化错误
        /// </summary>
        [Description("持久化错误")]
        PersistenceError = -2,

        /// <summary>
        /// 连接已断开 
        /// </summary>
        [Description("连接已断开")]
        Disconnected = -3,

        /// <summary>
        /// 超过最大消息飞行窗口
        /// </summary>
        [Description("超过最大消息飞行窗口")]
        MaxMessagesInflight = -4,

        /// <summary>
        /// 无效的utf8字符串
        /// </summary>
        [Description("无效的utf8字符串")]
        BadUtf8String = -5,

        /// <summary>
        /// 参数为NULL
        /// </summary>
        [Description("参数为NULL")]
        NullParameter = -6,

        /// <summary>
        /// 主题被截断
        /// </summary>
        [Description("主题被截断")]
        TopicnameTruncated = -7,

        /// <summary>
        /// 结构体版本号或id问题
        /// </summary>
        [Description("结构体版本号或id问题")]
        BadStructure = -8,

        /// <summary>
        /// qos无效
        /// </summary>
        [Description("qos无效")]
        BadQos = -9,

        /// <summary>
        /// 无更多的消息id可以使用
        /// </summary>
        [Description("无更多的消息id可以使用")]
        NoMoreMsgids = -10,

        /// <summary>
        /// 操作未完成
        /// </summary>
        [Description("操作未完成")]
        OperationIncomplete = -11,

        /// <summary>
        /// 消息过大
        /// </summary>
        [Description("消息过大")]
        MaxBufferedMessages = -12,

        /// <summary>
        /// 未支持SSL
        /// </summary>
        [Description("未支持SSL")]
        SslNotSupported = -13
    }
}
