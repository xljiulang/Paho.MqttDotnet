using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示mqtt客户端异常
    /// </summary>
    public class MqttException : Exception
    {
        /// <summary>
        /// 保存错误码的提示消息
        /// </summary>
        private static readonly IDictionary<MqttError, string> errorMessages;

        /// <summary>
        /// 获取mqtt错误码
        /// </summary>
        public MqttError Error { get; private set; }

        /// <summary>
        /// 静态构造器
        /// </summary>
        static MqttException()
        {
            var type = typeof(MqttError);
            var q = from e in Enum.GetValues(type).Cast<MqttError>()
                    let field = type.GetField(e.ToString())
                    let attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute
                    select new { key = e, value = attribute.Description };

            MqttException.errorMessages = q.ToDictionary(kv => kv.key, kv => kv.value);
        }

        /// <summary>
        /// mqtt客户端异常
        /// </summary>
        /// <param name="error">异常码</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MqttException(MqttError error) :
            base(MqttException.GetEerrorMessage(error))
        {
            this.Error = error;
        }

        /// <summary>
        /// mqtt客户端异常
        /// </summary>
        /// <param name="error">异常码</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MqttException(int error) :
            this((MqttError)error)
        {
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="error"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        private static string GetEerrorMessage(MqttError error)
        {
            var message = default(string);
            if (MqttException.errorMessages.TryGetValue(error, out message) == false)
            {
                throw new ArgumentOutOfRangeException();
            }
            return message;
        }
    }
}
