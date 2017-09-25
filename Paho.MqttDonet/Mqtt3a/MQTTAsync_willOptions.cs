using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct MQTTAsync_willOptions : IDisposable
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] struct_id;

        public int struct_version;

        public IntPtr topicName;

        public IntPtr message;

        public int retained;

        public int qos;

        public int len;

        public IntPtr dataOfmessage;

        public void Dispose()
        {
            Marshal.FreeHGlobal(this.topicName);
            Marshal.FreeHGlobal(this.message);
        }

        public static MQTTAsync_willOptions Init()
        {
            return new MQTTAsync_willOptions
            {
                struct_id = new[] { 'M', 'Q', 'T', 'W' },
            };
        }
    }
}
