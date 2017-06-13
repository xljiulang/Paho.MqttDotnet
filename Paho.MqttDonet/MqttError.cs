using System;
using System.Collections.Generic;
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
        Success = 0,
        /// <summary>
        /// 失败
        /// </summary>
        Failure = -1,
        /// <summary>
        /// 持久化错误
        /// </summary>
        PersistenceError = -2,
        /// <summary>
        /// 连接已断开 
        /// </summary>
        Disconnected = -3,

        /// <summary>
        /// 超过最大消息飞行窗口
        /// </summary>
        MaxMessagesInflight = -4,

        /// <summary>
        /// 无效的utf8字符串
        /// </summary>
        BadUtf8String = -5,

        /// <summary>
        /// 参数为NULL
        /// </summary>
        NullParameter = -6,

        /// <summary>
        /// 主题被截断
        /// </summary>
        TopicnameTruncated = -7,

        /// <summary>
        /// 结构体版本号或id问题
        /// </summary>
        BadStructure = -8,

        /// <summary>
        /// qos无效
        /// </summary>
        BadQos = -9,

        /// <summary>
        /// 无更多的消息id可以使用
        /// </summary>
        NoMoreMsgids = -10,

        /// <summary>
        /// 操作未完成
        /// </summary>
        OperationIncomplete = -11,

        /// <summary>
        /// 消息过大
        /// </summary>
        MaxBufferedMessages = -12,
    }
}
