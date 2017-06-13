using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 定义任务行为接口
    /// </summary>
    interface ITaskSetter
    {
        /// <summary>
        /// 设置任务的行为结果
        /// </summary>     
        /// <param name="value">数据值</param>   
        /// <returns></returns>
        bool SetResult(object value);

        /// <summary>
        /// 设置设置为异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        bool SetException(Exception ex);
    }

    /// <summary>
    /// 定义任务行为接口
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    interface ITaskSetter<TResult> : ITaskSetter
    {
        /// <summary>
        /// 同步获取任务结果
        /// </summary>
        /// <returns></returns>
        TResult GetResult();

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        Task<TResult> GetTask();
    }
}
