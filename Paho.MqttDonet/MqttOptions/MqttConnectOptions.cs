using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示Mqtt连接选项
    /// </summary>
    unsafe class MqttConnectOptions : MqttOptionsBase<MQTTAsync_connectOptions>
    {
        /// <summary>
        /// 空指针
        /// </summary>
        private static readonly void* NULL = (void*)0;

        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="successData">数据</param>
        protected override void MQTTAsync_onSuccess(IntPtr context, void* successData)
        {
            var code = ConnectError.ConnectionAccepted;
            base.RaiseOnCompleted(context, code);
        }

        /// <summary>
        /// 执行失败
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="successData">数据</param>
        protected override void MQTTAsync_onFailure(IntPtr context, MQTTAsync_failureData* failureData)
        {
            if (failureData == NULL)
            {
                var ex = new MqttException(MqttError.Failure);
                base.RaiseOnException(context, ex);
            }
            else if (failureData->code < 0)
            {
                var ex = new MqttException(failureData->code);
                base.RaiseOnException(context, ex);
            }
            else
            {
                var code = (ConnectError)failureData->code;
                base.RaiseOnCompleted(context, code);
            }
        }
    }
}
