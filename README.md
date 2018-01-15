# Paho.MqttDonet
A .Net wrapper for eclipse/paho.mqtt.c, support async/await asynchronous

## Dependencies
* eclipse/paho.mqtt.c
* vs2012/.net framework 4.0 or later
* Visual C++ Redistributable 2015

## Support
* Asp.net / WCF 
* WinForm / WPF
* Console / Service

## Demo
```c#
async static Task DemoAsync()
{
    // Trace
    MqttClient.SetTraceLevel(MqttTraceLevels.Protocol);
    MqttClient.SetTraceCallback((level, message) => Console.WriteLine(message));

    // Listening message
    var client = new MqttClient("mqtt://127.0.0.1", "clientId");
    client.OnMessageArrived += (sender, topic, message) =>
    {
        Console.WriteLine("got message " + message);
        var msg = new MqttMessage(message.QoS, "from MqttDotnet client");
        var mqClient = sender as MqttClient;
        mqClient.SendMessageAsync(topic, msg);
    };

    // Process Connection Lost
    client.OnConnectionLost += async (sender) =>
    {
        var mqClient = sender as MqttClient;
        while (mqClient.IsConnected == false)
        {
            await mqClient.ReConnectAsync();
        }
        await client.SubscribeAsync("mqtt/dotnet/xljiulang", MqttQoS.ExactlyOnce);
    };

    // Connect & subscribe
    await client.ConnectAsync(new ConnectOption
    {
        Username = "MqttDotnet",
        Password = "123456",
    });
    await client.SubscribeAsync("mqtt/dotnet/xljiulang", MqttQoS.ExactlyOnce);
}
```
