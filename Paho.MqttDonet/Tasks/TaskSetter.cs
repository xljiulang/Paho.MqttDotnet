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
        private TaskSetterSync<TResult> syncSetter;

        /// <summary>
        /// 异步设置器
        /// </summary>
        private TaskSetterAsync<TResult> asyncSetter;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 同步获取任务结果
        /// </summary>
        /// <returns></returns>
        public TResult GetResult()
        {
            lock (this.syncRoot)
            {
                this.isAsync = false;
                this.syncSetter = new TaskSetterSync<TResult>();
                return this.syncSetter.GetResult();
            }
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        public Task<TResult> GetTask()
        {
            lock (this.syncRoot)
            {
                this.isAsync = true;
                this.asyncSetter = new TaskSetterAsync<TResult>();
                return this.asyncSetter.GetTask();
            }
        }

        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetResult(object value)
        {
            lock (this.syncRoot)
            {
                if (this.isAsync == null)
                {
                    this.isAsync = true;
                    this.asyncSetter = new TaskSetterAsync<TResult>();
                }

                if (this.isAsync == true)
                {
                    return this.asyncSetter.SetResult(value);
                }
                else
                {
                    return this.syncSetter.SetResult(value);
                }
            }
        }

        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public bool SetException(Exception ex)
        {
            lock (this.syncRoot)
            {
                if (this.isAsync == null)
                {
                    this.isAsync = true;
                    this.asyncSetter = new TaskSetterAsync<TResult>();
                }

                if (this.isAsync == true)
                {
                    return this.asyncSetter.SetException(ex);
                }
                else
                {
                    return this.syncSetter.SetException(ex);
                }
            }
        }
    }
}
