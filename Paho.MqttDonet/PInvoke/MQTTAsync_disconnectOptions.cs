using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct MQTTAsync_disconnectOptions : IMQTTAsync_options
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] struct_id;

        public int struct_version;

        public int timeout;

        public IntPtr onSuccess;

        public IntPtr onFailure;

        public IntPtr context;

        public void Init()
        {
            struct_id = new[] { 'M', 'Q', 'T', 'D' };
        }

        public void SetContext(IntPtr value)
        {
            this.context = value;
        }

        void IMQTTAsync_options.SetCallbacks(MQTTAsync_onSuccess success, MQTTAsync_onFailure failure)
        {
            this.onSuccess = Marshal.GetFunctionPointerForDelegate(success);
            this.onFailure = Marshal.GetFunctionPointerForDelegate(failure);
        }
    }
}
