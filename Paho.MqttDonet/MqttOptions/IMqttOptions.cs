using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paho.MqttDotnet
{
    /// <summary>
    ///  Options完成委托
    /// </summary>
    /// <param name="taskId">任务id</param>
    /// <param name="value">值</param>
    delegate void OptionCompletedHandler(int taskId, object value);

    /// <summary>
    /// Options异常委托
    /// </summary>
    /// <param name="taskId">任务id</param>
    /// <param name="ex">异常</param>
    delegate void OptionExceptionHandler(int taskId, Exception ex);


    /// <summary>
    /// 定义Mqtt选项的接口
    /// </summary>
    interface IMqttOptions
    {
        /// <summary>
        /// 设置完成委托
        /// </summary>
        /// <param name="action">委托</param>
        void OnCompleted(OptionCompletedHandler action);

        /// <summary>
        /// 设置异常委托
        /// </summary>
        /// <param name="action">委托</param>
        void OnException(OptionExceptionHandler action);
    }
}
