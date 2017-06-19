using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paho.MqttDotnet
{
    interface IMQTTAsync_options
    {
        void Init();

        void SetContext(IntPtr value);

        void SetCallbacks(MQTTAsync_onSuccess success, MQTTAsync_onFailure failure);
    }
}
