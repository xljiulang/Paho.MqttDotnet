using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct MQTTAsync_successDataConnect
    {       
        public int token;
        
        public IntPtr serverURI;

        public int MQTTVersion;

        public int sessionPresent;
    }
}
