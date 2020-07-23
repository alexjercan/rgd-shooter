using System;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.Packet
{
    public class WelcomePacketWriter
    {
        private readonly int _clientId;
        private readonly string _message;

        public WelcomePacketWriter(int clientId, string message)
        {
            _clientId = clientId;
            _message = message;
        }

        public byte[] WritePacket() =>
            new ByteArrayBuilder()
                .Write((int) ServerPacket.WelcomePacket)
                .Write(_message)
                .Write(_clientId)
                .InsertSize()
                .ToByteArray();
    }

    public class WelcomePacketReader
    {
        private readonly ByteArrayReader _byteArrayReader;
        
        public WelcomePacketReader(ByteArrayReader byteArrayReader)
        {
            _byteArrayReader = byteArrayReader;
        }
        
        public Tuple<int, string> ReadPacket()
        {
            var message = _byteArrayReader.ReadString();
            var clientId = _byteArrayReader.ReadInt();
            
            return new Tuple<int, string>(clientId, message);
        }
    }
    
    public class WelcomeReceivedWriter
    {
        private readonly int _clientId;
        private readonly string _username;

        public WelcomeReceivedWriter(int clientId, string username)
        {
            _clientId = clientId;
            _username = username;
        }
        
        public byte[] WritePacket() =>
            new ByteArrayBuilder()
                .Write((int) ClientPacket.WelcomeReceived)
                .Write(_clientId)
                .Write(_username)
                .InsertSize()
                .ToByteArray();
    }
    
    public class WelcomeReceivedReader
    {
        private readonly ByteArrayReader _byteArrayReader;
        
        public WelcomeReceivedReader(ByteArrayReader byteArrayReader)
        {
            _byteArrayReader = byteArrayReader;
        }
        
        public Tuple<int, string> ReadPacket()
        {
            var clientId = _byteArrayReader.ReadInt();
            var username = _byteArrayReader.ReadString();
            
            return new Tuple<int, string>(clientId, username);
        }
    }
}