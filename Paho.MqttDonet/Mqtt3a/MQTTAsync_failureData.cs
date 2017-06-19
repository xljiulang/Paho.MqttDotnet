using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct MQTTAsync_failureData
    {
        public int token;

        public int code;

        public IntPtr message;
    }
}
