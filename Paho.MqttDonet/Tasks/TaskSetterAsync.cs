using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示任务行为
    /// </summary>
    /// <typeparam name="TResult">任务结果类型</typeparam>
    class TaskSetterAsync<TResult> : ITaskSetter<TResult>
    {
        /// <summary>
        /// 任务源
        /// </summary>
        private readonly TaskCompletionSource<TResult> taskSource;


        /// <summary>
        /// 获取所创建的任务
        /// </summary>
        public Task<TResult> Task
        {
            get
            {
                return this.taskSource.Task;
            }
        }


        /// <summary>
        /// 任务行为
        /// </summary>
        public TaskSetterAsync()
        {
            this.taskSource = new TaskCompletionSource<TResult>();
        }

        /// <summary>
        /// 设置任务的行为结果
        /// </summary>     
        /// <param name="value">数据值</param>   
        /// <returns></returns>
        public bool SetResult(object value)
        {
            return this.taskSource.TrySetResult((TResult)value);
        }

        /// <summary>
        /// 设置设置为异常
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public bool SetException(Exception ex)
        {
            return this.taskSource.TrySetException(ex);
        }


        /// <summary>
        /// 获取同步结果
        /// </summary>
        /// <returns></returns>
        public TResult GetResult()
        {
            return this.GetTask().Result;
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        public Task<TResult> GetTask()
        {
            return this.taskSource.Task;
        }
    }
}
