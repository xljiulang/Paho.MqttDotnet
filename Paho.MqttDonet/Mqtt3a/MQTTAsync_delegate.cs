using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paho.MqttDotnet
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    unsafe delegate void MQTTAsync_onFailure(IntPtr context, MQTTAsync_failureData* failureData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    unsafe delegate void MQTTAsync_onSuccess(IntPtr context, void* successData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void MQTTAsync_connectionLost(IntPtr context, [MarshalAs(UnmanagedType.LPStr)]string cause);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int MQTTAsync_messageArrived(IntPtr context, IntPtr topicName, int topicLen, IntPtr message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void MQTTAsync_deliveryComplete(IntPtr context, int token);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void MQTTAsync_traceCallback(MqttTraceLevels level, [MarshalAs(UnmanagedType.LPStr)]string message);
}
