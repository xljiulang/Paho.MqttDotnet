using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示mqtt持久化方式
    /// </summary>
    public enum MqttPersistence
    {
        /// <summary>
        /// 默认方式，文件持久化
        /// </summary>
        Default = 0,

        /// <summary>
        /// 不持久化，内存保存
        /// </summary>
        None = 1,

        /// <summary>
        /// 自定义化方式        
        /// </summary>
        User = 2,
    }
}
