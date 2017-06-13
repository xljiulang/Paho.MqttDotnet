using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示Mqtt选项抽象
    /// </summary>
    /// <typeparam name="TOption"></typeparam>
    unsafe abstract class MqttOptions<TOption> : IMqttOptions
        where TOption : IMQTTAsync_options, new()
    {
        /// <summary>
        /// 完成委托
        /// </summary>
        private Action<IntPtr, object> complectedAction;

        /// <summary>
        /// 异常委托
        /// </summary>
        private Action<IntPtr, Exception> exceptionAction;

        /// <summary>
        /// 执行成功委托
        /// </summary>
        private readonly MQTTAsync_onSuccess onSuccess;

        /// <summary>
        /// 执行失败委托
        /// </summary>
        private readonly MQTTAsync_onFailure onFailure;

        /// <summary>
        /// Mqtt选项抽象
        /// </summary>
        public MqttOptions()
        {
            this.onSuccess = new MQTTAsync_onSuccess(this.MQTTAsync_onSuccess);
            this.onFailure = new MQTTAsync_onFailure(this.MQTTAsync_onFailure);
        }

        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="successData">数据</param>
        protected abstract void MQTTAsync_onSuccess(IntPtr context, void* successData);


        /// <summary>
        /// 执行失败
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="failureData">数据</param>
        protected abstract void MQTTAsync_onFailure(IntPtr context, MQTTAsync_failureData* failureData);

        /// <summary>
        /// 触发执行完成
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="value">结果</param>
        protected void RaiseOnCompleted(IntPtr context, object value)
        {
            if (this.complectedAction != null)
            {
                this.complectedAction.Invoke(context, value);
            }
        }

        /// <summary>
        /// 触发执行异常
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="ex">异常</param>
        protected void RaiseOnException(IntPtr context, Exception ex)
        {
            if (this.exceptionAction != null)
            {
                this.exceptionAction.Invoke(context, ex);
            }
        }

        /// <summary>
        /// 转换为结构体
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="opt">要合并的结构</param>
        /// <returns></returns>
        public TOption ToStruct(int taskId, TOption opt)
        {
            opt.SetContext(new IntPtr(taskId));
            opt.SetCallbacks(this.onSuccess, this.onFailure);
            return opt;
        }

        /// <summary>
        /// 转换为结构体
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <returns></returns>
        public TOption ToStruct(int taskId)
        {
            var opt = new TOption();
            opt.Init();
            opt.SetContext(new IntPtr(taskId));
            opt.SetCallbacks(this.onSuccess, this.onFailure);
            return opt;
        }

        /// <summary>
        /// 设置完成的委托
        /// </summary>
        /// <param name="action"></param>
        void IMqttOptions.OnCompleted(Action<IntPtr, object> action)
        {
            this.complectedAction = action;
        }

        /// <summary>
        /// 设置异常委托
        /// </summary>
        /// <param name="action"></param>
        void IMqttOptions.OnException(Action<IntPtr, Exception> action)
        {
            this.exceptionAction = action;
        }
    }
}
