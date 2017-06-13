using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示mqtt连接错误码
    /// </summary>
    public enum ConnectError
    {
        /// <summary>
        /// 连接已被服务端接受
        /// </summary>
        ConnectionAccepted = 0x00,

        /// <summary>
        /// 服务端不支持客户端请求的MQTT协议级别
        /// </summary>
        UnacceptedProtocolVersion = 0x01,

        /// <summary>
        /// 客户端标识符是正确的UTF-8编码，但服务端不允许使用
        /// </summary>
        IdentifierRejected = 0x02,

        /// <summary>
        /// 网络连接已建立，但MQTT服务不可用
        /// </summary>
        BrokerUnavailable = 0x03,

        /// <summary>
        /// 用户名或密码的数据格式无效
        /// </summary>
        BadUsernameOrPassword = 0x04,

        /// <summary>
        /// 客户端未被授权连接到此服务器
        /// </summary>
        NotAuthorized = 0x05
    }
}
