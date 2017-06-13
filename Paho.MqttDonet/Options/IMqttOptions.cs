using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 定义Mqtt选项的接口
    /// </summary>
    interface IMqttOptions
    {
        /// <summary>
        /// 设置完成委托
        /// </summary>
        /// <param name="action">委托</param>
        void OnCompleted(Action<IntPtr, object> action);

        /// <summary>
        /// 设置异常委托
        /// </summary>
        /// <param name="action">委托</param>
        void OnException(Action<IntPtr, Exception> action);
    }
}
