using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Paho.MqttDotnet
{

    /// <summary>
    /// 表示阻塞的任务行为
    /// </summary>
    /// <typeparam name="TResult">结果类型</typeparam>
    class TaskSetterSync<TResult> : ITaskSetter, IDisposable
    {
        /// <summary>
        /// 结果值
        /// </summary>
        private TResult result;

        /// <summary>
        /// 异常值 
        /// </summary>
        private Exception exception;

        /// <summary>
        /// 通知事件
        /// </summary>
        private readonly AutoResetEvent resetEvent = new AutoResetEvent(false);


        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetResult(object value)
        {
            this.result = (TResult)value;
            return this.resetEvent.Set();
        }

        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="ex">异常</param>       
        /// <returns></returns>
        public bool SetException(Exception ex)
        {
            this.exception = ex;
            return this.resetEvent.Set();
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        public TResult GetResult()
        {
            this.resetEvent.WaitOne();
            if (this.exception != null)
            {
                throw exception;
            }
            return this.result;
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        public Task<TResult> GetTask()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.resetEvent.Dispose();
        }
    }
}
