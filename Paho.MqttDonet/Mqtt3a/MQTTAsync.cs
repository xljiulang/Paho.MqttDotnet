using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Web;

namespace Paho.MqttDotnet
{
    static class MQTTAsync
    {
        private const string mqtt3a_dll = "paho-mqtt3a.dll";

        static MQTTAsync()
        {
            LibraryLoader.Load(mqtt3a_dll);
        }

        public static IntPtr ToUnmanagedPointer(this string str)
        {
            return Marshal.StringToHGlobalAnsi(str);
        }


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern MqttError MQTTAsync_create(
            ref IntPtr handle,
            [MarshalAs(UnmanagedType.LPStr)] string serverURI,
            [MarshalAs(UnmanagedType.LPStr)] string clientId,
            MqttPersistence persistence_type,
            IntPtr persistence_context);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MQTTAsync_destroy(
            ref IntPtr handle);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern MqttError MQTTAsync_connect(
            IntPtr handle,
            ref MQTTAsync_connectOptions options);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern MqttError MQTTAsync_setCallbacks(
            IntPtr handle,
            IntPtr context,
            MQTTAsync_connectionLost connectionLost,
            MQTTAsync_messageArrived messageArrived,
            MQTTAsync_deliveryComplete deliveryComplete);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern MqttError MQTTAsync_disconnect(
            IntPtr handle,
            ref MQTTAsync_disconnectOptions options);



        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern MqttError MQTTAsync_subscribe(
            IntPtr handle,
            [MarshalAs(UnmanagedType.LPStr)]string topic,
            MqttQoS qos,
            ref MQTTAsync_responseOptions responseOption);



        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern MqttError MQTTAsync_subscribeMany(
             IntPtr handle,
             int count,
             IntPtr* topic,
             int* qos,
             ref MQTTAsync_responseOptions response);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern MqttError MQTTAsync_unsubscribe(
            IntPtr handle,
            [MarshalAs(UnmanagedType.LPStr)]string topic,
            ref MQTTAsync_responseOptions response);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern MqttError MQTTAsync_unsubscribeMany(
              IntPtr handle,
              int count,
              IntPtr* topic,
              ref MQTTAsync_responseOptions response);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern MqttError MQTTAsync_sendMessage(
            IntPtr handle,
            [MarshalAs(UnmanagedType.LPStr)]string destinationName,
            ref MQTTAsync_message msg,
            ref MQTTAsync_responseOptions response);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MQTTAsync_freeMessage(
            ref IntPtr msg);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MQTTAsync_free(
            IntPtr ptr);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MQTTAsync_isConnected(
            IntPtr handle);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MQTTAsync_setTraceLevel(MqttTraceLevels level);


        [DllImport(mqtt3a_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MQTTAsync_setTraceCallback(MQTTAsync_traceCallback callback);
    }
}
