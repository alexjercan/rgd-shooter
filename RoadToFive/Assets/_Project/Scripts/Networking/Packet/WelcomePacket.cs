using System;

namespace _Project.Scripts.Networking.Packet
{
    public class WelcomePacketReader
    {
        private readonly byte[] _data;
        
        public WelcomePacketReader(byte[] data)
        {
            _data = data;
        }
        
        public Tuple<int, int> ReadPacket()
        {
            var packetReader = new PacketReader(_data);
            var message = packetReader.ReadInt();
            var client = packetReader.ReadInt();
            
            return new Tuple<int, int>(client, message);
        }
    }
    
    public class WelcomePacketWriter
    {
        private readonly int _clientId;
        private readonly int _message;
        private readonly int _packetLength;

        public WelcomePacketWriter(int clientId, int message)
        {
            _clientId = clientId;
            _message = message;
        }

        public byte[] WritePacket() =>
            new PacketBuilder()
                .Write((int) ServerPacket.WelcomePacket)
                .Write(_message)
                .Write(_clientId)
                .InsertSize()
                .ToByteArray();
    }
}