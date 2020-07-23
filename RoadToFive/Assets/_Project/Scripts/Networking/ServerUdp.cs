using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Datagram;
using _Project.Scripts.Networking.UDP;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking
{
    public class ServerUdp
    {
        public int MaxPlayerCount { get; set; }
        public int Port { get; set; }
        
        private readonly Dictionary<int, UdpBinding> _connections = new Dictionary<int, UdpBinding>();
        private readonly UdpClient _udpListener;
        
        public ServerUdp(int maxPlayerCount, int port)
        {
            MaxPlayerCount = maxPlayerCount;
            Port = port;

            InitializeServerData();
            
            _udpListener = new UdpClient(port);
        }

        public void Listen()
        {
            Logger.Info($"Started listening on port {Port}");
            _udpListener.BeginReceive(UdpReceiveCallback, null);
        }
        
        private void UdpReceiveCallback(IAsyncResult asyncResult)
        {
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var data = _udpListener.EndReceive(asyncResult, ref clientEndPoint);
            _udpListener.BeginReceive(UdpReceiveCallback, null);
            
            if (data.Length < 4) return;
            
            var byteArrayReader = new ByteArrayReader(data);
            var clientId = byteArrayReader.ReadInt();
            
            if (clientId == 0)
            {
                for (var i = 1; i <= MaxPlayerCount; i++)
                {
                    if (_connections[i].EndPoint != null) continue;
                
                    _connections[i].Bind(clientEndPoint);
                    SendDatagram(clientEndPoint, new WelcomeDatagramWriter(i).WriteDatagram());
                    return;
                }
            }
            else if (_connections[clientId].EndPoint.ToString() == clientEndPoint.ToString()) 
            {
                var packetLength = byteArrayReader.ReadInt();

                var packetBytes = byteArrayReader.ReadBytes(packetLength);
                MainThreadScheduler.EnqueueOnMainThread(() => ReadDatagram(packetBytes));
                return;
            }
            
            Logger.Warning($"{clientEndPoint} failed to connect: Server full!");
        }
        
        public void SendDatagram(IPEndPoint clientEndPoint, byte[] data)
        {
            if (clientEndPoint == null) return;
            _udpListener.BeginSend(data, data.Length, clientEndPoint, null, null);
        }
        
        private void InitializeServerData()
        {
            for (var i = 1; i <= MaxPlayerCount; i++) _connections.Add(i, new UdpBindingServer());
        }

        private void ReadDatagram(byte[] data)
        {
            var datagramReader = new ByteArrayReader(data);
            var datagramType = (ClientDatagram)datagramReader.ReadInt();

            switch (datagramType)
            {
                case ClientDatagram.InvalidDatagram:
                    break;
                case ClientDatagram.WelcomeReceived:
                    HandleWelcomeReceived(datagramReader);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeReceived(ByteArrayReader byteArrayReader)
        {
            var message = new WelcomeReceivedDatagramReader(byteArrayReader).ReadDatagram();
            
            Logger.Info(message);
        }
    }
}