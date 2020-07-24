using System;
using System.Net.Sockets;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.TCP
{
    public abstract class TransmissionControlProtocolSocket
    {
        public event EventHandler<ByteArrayReader> ReceivedPacket;
        
        public TcpClient Socket { get; protected set; }

        protected const int DataBufferSize = 4096;

        protected NetworkStream NetworkStream;
        protected byte[] ReceivedBuffer;
        private ByteArrayReader _receivedByteArrayReader = new ByteArrayReader();

        public abstract void Connect(TcpClient socket);

        public void SendPacket(byte[] packet)
        {
            if (Socket == null) return;
            NetworkStream.BeginWrite(packet, 0, packet.Length, null, null);
        }
        
        protected void ReceiveCallback(IAsyncResult asyncResult)
        {
            var byteLength = NetworkStream.EndRead(asyncResult);
            if (byteLength <= 0) return; //TODO: DISCONNECT

            var data = new byte[byteLength];
            Array.Copy(ReceivedBuffer, data, byteLength);

            if (ReceivedDataHandler(data)) _receivedByteArrayReader = new ByteArrayReader();
            NetworkStream.BeginRead(ReceivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        private bool ReceivedDataHandler(byte[] receivedData)
        {
            var packetLength = 0;
            _receivedByteArrayReader.AddBytes(receivedData);
            
            if (_receivedByteArrayReader.UnreadBytes >= sizeof(int))
            {
                packetLength = _receivedByteArrayReader.ReadInt();
                if (packetLength <= 0) return true;
            }

            while (packetLength > 0 && packetLength <= _receivedByteArrayReader.UnreadBytes)
            {
                var bytes = _receivedByteArrayReader.ReadBytes(packetLength);
                OnReceivedDatagram(new ByteArrayReader(bytes));
                
                packetLength = 0;
                if (_receivedByteArrayReader.UnreadBytes < sizeof(int)) continue;
                packetLength = _receivedByteArrayReader.ReadInt();
                if (packetLength <= 0) return true;
            }

            return packetLength <= 1;
        }
        
        private void OnReceivedDatagram(ByteArrayReader e) => ReceivedPacket?.Invoke(this, e);
    }
}