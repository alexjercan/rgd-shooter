using System;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.Datagram
{
    public class WelcomeDatagramWriter
    {
        private readonly int _clientId;

        public WelcomeDatagramWriter(int clientId)
        {
            _clientId = clientId;
        }
        
        public byte[] WriteDatagram() =>
            new ByteArrayBuilder()
                .Write((int) ServerDatagram.WelcomeDatagram)
                .Write(_clientId)
                .InsertSize()
                .ToByteArray();
    }

    public class WelcomeDatagramReader
    {
        private readonly ByteArrayReader _byteArrayReader;
        
        public WelcomeDatagramReader(ByteArrayReader byteArrayReader)
        {
            _byteArrayReader = byteArrayReader;
        }
        
        public int ReadDatagram()
        {
            var clientId = _byteArrayReader.ReadInt();

            return clientId;
        }
    }
    
    public class WelcomeReceivedDatagramWriter
    {
        private readonly int _clientId;
        private readonly string _username;

        public WelcomeReceivedDatagramWriter(int clientId, string username)
        {
            _clientId = clientId;
            _username = username;
        }
        
        public byte[] WriteDatagram() =>
            new ByteArrayBuilder()
                .Write((int) ClientDatagram.WelcomeReceived)
                .Write(_username)
                .InsertSize()
                .Insert(_clientId)
                .ToByteArray();
    }
    
    public class WelcomeReceivedDatagramReader
    {
        private readonly ByteArrayReader _byteArrayReader;
        
        public WelcomeReceivedDatagramReader(ByteArrayReader byteArrayReader)
        {
            _byteArrayReader = byteArrayReader;
        }
        
        public string ReadDatagram()
        {
            var username = _byteArrayReader.ReadString();
            
            return username;
        }
    }
}