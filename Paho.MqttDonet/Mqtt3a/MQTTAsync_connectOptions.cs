using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Paho.MqttDotnet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct MQTTAsync_connectOptions : IMQTTAsync_options
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] struct_id;

        public int struct_version;

        public int keepAliveInterval;

        public int cleansession;

        public int maxInflight;

        public IntPtr will;

        public IntPtr username;

        public IntPtr password;

        public int connectTimeout;

        public int retryInterval;

        public IntPtr ssl;

        public IntPtr onSuccess;

        public IntPtr onFailure;

        public IntPtr context;

        public int serverURIcount;

        public IntPtr serverURIs;

        public int MQTTVersion;

        public int automaticReconnect;

        public int minRetryInterval;

        public int maxRetryInterval;

        public int len;

        public IntPtr dataOfPassword;

        public void Init()
        {
            struct_id = new[] { 'M', 'Q', 'T', 'C' };
            struct_version = 4;
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
