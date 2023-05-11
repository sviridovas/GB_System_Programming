using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    class CustomConnnection {
        public int id;
        public string name;
    }

    private const int MAX_CONNECTION = 10;
    private int port = 5805;
    private int hostID;
    private int reliableChannel;
    private bool isStarted = false;
    private byte error;
    List<CustomConnnection> connections = new List<CustomConnnection>();
    public void StartServer()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        // cc.ConnectTimeout = 500;
        // cc.MaxConnectionAttempt = 2;

        reliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(cc, MAX_CONNECTION);
        hostID = NetworkTransport.AddHost(topology, port);
        isStarted = true;
    }
    public void ShutDownServer()
    {
        if (!isStarted) return;
        NetworkTransport.RemoveHost(hostID);
        NetworkTransport.Shutdown();
        isStarted = false;
    }
    void Update()
    {
        if (!isStarted) return;
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out
        channelId, recBuffer, bufferSize, out dataSize, out error);
        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.ConnectEvent:
                    connections.Add(new CustomConnnection() {id = connectionId});
                    SendMessageToAll($"Player {connectionId} has connected.");
                    Debug.Log($"Player {connectionId} has connected.");
                    break;
                case NetworkEventType.DataEvent: {
                    // string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    var message = CustomMessage.ByteArraToObject(recBuffer, 0, dataSize);
                    if(message == null) {
                        Debug.Log($"Не опознаное сообщение");
                        break;
                    }

                    var conn = connections.Find(connection => connection.id == connectionId);

                    switch(message.type){
                        case "Name":
                            conn.name = message.message;

                            SendMessageToAll($"Player {connectionId}: сменил имя на {conn.name}");
                            Debug.Log($"Player {connectionId}: сменил имя на {conn.name}");
                        break;
                        case "Message":
                            SendMessageToAll($"Player {conn.name}: {message.message}");
                            Debug.Log($"Player {conn.name}: {message.message}");
                        break;
                    }

                    break;
                }
                case NetworkEventType.DisconnectEvent:
                {
                    var conn = connections.Find(connection => connection.id == connectionId);
                    var name = conn.name;

                    if(conn != null)
                        connections.Remove(conn);

                    SendMessageToAll($"Player {name} has disconnected.");
                    Debug.Log($"Player {name} has disconnected.");
                    break;
                }
                case NetworkEventType.BroadcastEvent:
                    break;
            }
            recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
            bufferSize, out dataSize, out error);
        }
    }
    public void SendMessageToAll(string message)
    {
        for (int i = 0; i < connections.Count; i++)
        {
            SendMessage(message, connections[i].id);
        }
    }
    public void SendMessage(string message, int connectionID)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostID, connectionID, reliableChannel, buffer, message.Length * sizeof(char), out error);
        if ((NetworkError)error != NetworkError.Ok) Debug.Log((NetworkError)error);
    }
}
