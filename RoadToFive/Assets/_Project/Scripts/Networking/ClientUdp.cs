using System.Net;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Datagram;
using _Project.Scripts.Networking.UDP;

namespace _Project.Scripts.Networking
{
    public class ClientUdp
    {
        private int _clientId;
        private UdpBinding _udpBinding;
        
        private readonly string _serverIp;
        private readonly int _port;
        
        public ClientUdp(string serverIp, int port)
        {
            _clientId = 0;
            _serverIp = serverIp;
            _port = port;
        }

        public void BindToServer(int localPort)
        {
            _udpBinding = new UdpBindingClient(this, _serverIp, _port);
            _udpBinding.Bind(new IPEndPoint(IPAddress.None, localPort));

            SendDatagram(new ByteArrayBuilder().Write(_clientId).ToByteArray());
        }
        
        public void SendDatagram(byte[] data) => 
            _udpBinding.Socket?.BeginSend(data, data.Length, null, null);
        
        public void ReadDatagram(byte[] data)
        {
            var datagramReader = new ByteArrayReader(data);
            var datagramType = (ServerDatagram)datagramReader.ReadInt();

            switch (datagramType)
            {
                case ServerDatagram.InvalidDatagram:
                    break;
                case ServerDatagram.WelcomeDatagram:
                    HandleWelcomeDatagram(datagramReader);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeDatagram(ByteArrayReader byteArrayReader)
        {
            _clientId= new WelcomeDatagramReader(byteArrayReader).ReadDatagram();
            Logger.Info(_clientId.ToString());
            
            SendDatagram(new WelcomeReceivedDatagramWriter(_clientId, "guest " + _clientId).WriteDatagram());
        }
    }
}