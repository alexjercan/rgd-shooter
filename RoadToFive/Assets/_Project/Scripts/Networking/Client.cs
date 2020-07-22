using System.Net.Sockets;
using _Project.Scripts.Networking.TCP;

namespace _Project.Scripts.Networking
{
    public class Client
    {
        private TcpConnection _tcpConnection;

        public void ConnectToServer(string ip, int port)
        {
            _tcpConnection = new TcpConnectionClient(ip, port);
            _tcpConnection.Connect(new TcpClient());
        }
    }
}