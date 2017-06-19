using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct MQTTAsync_message : IDisposable
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] struct_id;

        public int struct_version;

        public int payloadlen;

        public IntPtr payload;

        public int qos;

        public int retained;

        public int dup;

        public int msgid;

        public static MQTTAsync_message Init()
        {
            return new MQTTAsync_message { struct_id = new[] { 'M', 'Q', 'T', 'M' } };
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(this.payload);
        }
    }
}
