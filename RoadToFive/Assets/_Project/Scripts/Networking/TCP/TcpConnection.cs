using System;
using System.Net.Sockets;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.Packet;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking.TCP
{
    public abstract class TcpConnection
    {
        public TcpClient Socket { get; protected set; }

        protected const int DataBufferSize = 4096;

        protected NetworkStream NetworkStream;
        protected byte[] ReceivedBuffer;
        protected PacketReader ReceivedPacketReader = new PacketReader();

        public abstract void Connect(TcpClient socket);
        public abstract void SendPacket(byte[] packet);

        protected void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                var byteLength = NetworkStream.EndRead(asyncResult);
                if (byteLength <= 0) return; //TODO: DISCONNECT

                var data = new byte[byteLength];
                Array.Copy(ReceivedBuffer, data, byteLength);

                if (ReceivePacket(data)) ReceivedPacketReader = new PacketReader();
                NetworkStream.BeginRead(ReceivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

        private bool ReceivePacket(byte[] receivedData)
        {
            Logger.Info("Received packet");
            var packetLength = 0;
            ReceivedPacketReader.AddBytes(receivedData);
            
            if (ReceivedPacketReader.UnreadBytes >= sizeof(int))
            {
                packetLength = ReceivedPacketReader.ReadInt();
                if (packetLength <= 0) return true;
            }

            while (packetLength > 0 && packetLength <= ReceivedPacketReader.UnreadBytes)
            {
                var packetBytes = ReceivedPacketReader.ReadBytes(packetLength);

                //TODO: HANDLE PACKET BYTES ON MAIN THREAD : PLACEHOLDER
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    var packetReader = new PacketReader(packetBytes);
                    var packetType = packetReader.ReadInt();
                    var bytes = packetReader.ReadBytes(packetReader.UnreadBytes);
                    var welcome = new WelcomePacketReader(bytes);
                    Logger.Info(welcome.ReadPacket().Item2.ToString());
                });
                
                packetLength = 0;
                if (ReceivedPacketReader.UnreadBytes < sizeof(int)) continue;
                packetLength = ReceivedPacketReader.ReadInt();
                if (packetLength <= 0) return true;
            }

            return packetLength <= 1;
        }
    }
}