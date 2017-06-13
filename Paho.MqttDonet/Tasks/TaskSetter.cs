using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示同步异步支持的任务设置器
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    class TaskSetter<TResult> : ITaskSetter<TResult>
    {
        /// <summary>
        /// 是否为异步
        /// </summary>
        private bool? isAsync;
        
        /// <summary>
        /// 同步设置器
        /// </summary>
        private TaskSetterSync<TResult> sync;

        /// <summary>
        /// 异步设置器
        /// </summary>
        private TaskSetterAsync<TResult> async;

        /// <summary>
        /// 同步获取任务结果
        /// </summary>
        /// <returns></returns>
        public TResult GetResult()
        {
            this.isAsync = false;
            this.sync = new TaskSetterSync<TResult>();
            return this.sync.GetResult();
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        public Task<TResult> GetTask()
        {
            this.isAsync = true;
            this.async = new TaskSetterAsync<TResult>();
            return this.async.GetTask();
        }

        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetResult(object value)
        {
            if (this.isAsync == null)
            {
                this.isAsync = true;
                this.async = new TaskSetterAsync<TResult>();
            }

            if (this.isAsync == true)
            {
                return this.async.SetResult(value);
            }
            else
            {
                return this.sync.SetResult(value);
            }
        }

        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public bool SetException(Exception ex)
        {
            if (this.isAsync == null)
            {
                this.isAsync = true;
                this.async = new TaskSetterAsync<TResult>();
            }

            if (this.isAsync == true)
            {
                return this.async.SetException(ex);
            }
            else
            {
                return this.sync.SetException(ex);
            }
        }
    }
}
