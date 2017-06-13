using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        /// -1 未知 0同步 1异步
        /// </summary>
        private long isAsync = -1L;

        /// <summary>
        /// 同步设置器
        /// </summary>
        private readonly Lazy<TaskSetterSync<TResult>> syncSetter = new Lazy<TaskSetterSync<TResult>>(() => new TaskSetterSync<TResult>(), true);

        /// <summary>
        /// 异步设置器
        /// </summary>
        private readonly Lazy<TaskSetterAsync<TResult>> asyncSetter = new Lazy<TaskSetterAsync<TResult>>(() => new TaskSetterAsync<TResult>(), true);


        /// <summary>
        /// 同步获取任务结果
        /// </summary>
        /// <returns></returns>
        public TResult GetResult()
        {
            var val = Interlocked.CompareExchange(ref this.isAsync, 0L, -1L);
            if (val == 1L)
            {
                return this.asyncSetter.Value.GetResult();
            }
            else
            {
                return this.syncSetter.Value.GetResult();
            }
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        public Task<TResult> GetTask()
        {
            Interlocked.Exchange(ref this.isAsync, 1L);
            return this.asyncSetter.Value.GetTask();
        }

        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetResult(object value)
        {
            var val = Interlocked.CompareExchange(ref this.isAsync, 1L, -1L);
            if (val == 0L)
            {
                return this.syncSetter.Value.SetResult(value);
            }
            else
            {
                return this.asyncSetter.Value.SetResult(value);
            }
        }

        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public bool SetException(Exception ex)
        {
            var val = Interlocked.CompareExchange(ref this.isAsync, 1L, -1L);
            if (val == 0L)
            {
                return this.syncSetter.Value.SetException(ex);
            }
            else
            {
                return this.asyncSetter.Value.SetException(ex);
            }
        }
    }
}
