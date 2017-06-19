using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示任务管理表
    /// 线程安全类型
    /// </summary>
    /// <typeparam name="T">任务ID类型</typeparam>
    [DebuggerDisplay("Count = {table.Count}")]
    class TaskSetterTable<T>
    {
        /// <summary>
        /// 任务行为字典
        /// </summary>
        private readonly ConcurrentDictionary<T, ITaskSetter> cached;

        /// <summary>
        /// 任务行为表
        /// </summary>
        public TaskSetterTable()
        {
            this.cached = new ConcurrentDictionary<T, ITaskSetter>();
        }

        /// <summary>
        /// 创建带id的任务并添加到列表中
        /// </summary>
        /// <typeparam name="TResult">任务结果类型</typeparam>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        public ITaskSetter<TResult> Create<TResult>(T id)
        {
            var taskSetter = new TaskSetter<TResult>();
            this.cached.TryAdd(id, taskSetter);
            return taskSetter;
        }

        /// <summary>      
        /// 获取并移除与id匹配的任务
        /// 如果没有匹配则返回null
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        public ITaskSetter Remove(T id)
        {
            ITaskSetter taskSetter;
            this.cached.TryRemove(id, out taskSetter);
            return taskSetter;
        }

        /// <summary>
        /// 清除所有任务
        /// </summary>
        public void Clear()
        {
            this.cached.Clear();
        }


        /// <summary>
        /// 表示同步异步支持的任务设置器
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        private class TaskSetter<TResult> : ITaskSetter<TResult>
        {
            /// <summary>
            /// 任务源
            /// </summary>
            private readonly TaskCompletionSource<TResult> taskSource;

            /// <summary>
            /// 任务行为
            /// </summary>
            public TaskSetter()
            {
                this.taskSource = new TaskCompletionSource<TResult>();
            }

            /// <summary>
            /// 设置任务的行为结果
            /// </summary>     
            /// <param name="value">数据值</param>   
            /// <returns></returns>
            bool ITaskSetter.SetResult(object value)
            {
                return this.taskSource.TrySetResult((TResult)value);
            }

            /// <summary>
            /// 设置任务的行为结果
            /// </summary>     
            /// <param name="value">数据值</param>   
            /// <returns></returns>
            public bool SetResult(TResult value)
            {
                return this.taskSource.TrySetResult(value);
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
                try
                {
                    return this.GetTask().Result;
                }
                catch (AggregateException ex)
                {
                    throw ex.InnerException;
                }
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
}
