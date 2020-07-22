using System;
using System.Net.Sockets;
using _Project.Scripts.Logging;

namespace _Project.Scripts.Networking.TCP
{
    public abstract class TcpConnection
    {
        public TcpClient Socket { get; protected set; }

        protected const int DataBufferSize = 4096;

        protected NetworkStream NetworkStream;
        protected byte[] ReceiveBuffer;

        public abstract void Connect(TcpClient socket);
        
        protected void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                var byteLength = NetworkStream.EndRead(asyncResult);
                if (byteLength <= 0) return;
                
                var data = new byte[byteLength];
                Array.Copy(ReceiveBuffer, data, byteLength);
                    
                NetworkStream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }
    }
}