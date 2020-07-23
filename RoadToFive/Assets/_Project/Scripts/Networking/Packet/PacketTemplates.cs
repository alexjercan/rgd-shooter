using System;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.Packet
{
    public static class PacketTemplates
    {
        #region server
        public static byte[] WriteWelcomePacket(int clientId, string message) =>
            new ByteArrayBuilder()
                .Write((int) ServerPacket.WelcomePacket)
                .Write(clientId)
                .Write(message)
                .InsertSize()
                .ToByteArray();
        
        public static Tuple<int, string> WriteWelcomeReceivedPacket(ByteArrayReader byteArrayReader) =>
            new Tuple<int, string>(byteArrayReader.ReadInt(), byteArrayReader.ReadString());
        
        #endregion

        #region client

        public static byte[] WriteWelcomeReceivedPacket(int clientId, string username) =>
            new ByteArrayBuilder()
                .Write((int) ClientPacket.WelcomeReceived)
                .Write(clientId)
                .Write(username)
                .InsertSize()
                .ToByteArray();

        public static Tuple<int, string> ReadWelcomePacket(ByteArrayReader byteArrayReader) =>
            new Tuple<int, string>(byteArrayReader.ReadInt(), byteArrayReader.ReadString());

        #endregion
    }
}