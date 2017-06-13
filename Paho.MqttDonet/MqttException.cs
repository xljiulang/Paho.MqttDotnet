using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示mqtt客户端异常
    /// </summary>
    public class MqttException : Exception
    {
        /// <summary>
        /// mqtt客户端异常
        /// </summary>
        /// <param name="error">异常码</param>
        public MqttException(MqttError error) :
            base(error.ToString())
        {
        }

        /// <summary>
        /// mqtt客户端异常
        /// </summary>
        /// <param name="error">异常码</param>
        public MqttException(int error) :
            this((MqttError)error)
        {
        }
    }
}
