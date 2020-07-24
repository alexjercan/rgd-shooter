using System;

namespace _Project.Scripts.Networking.ByteArray
{
    public static class MessageTemplates
    {
        
        public static byte[] WriteDummy(int id) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.Dummy)
                .Write(id)
                .InsertSize()
                .ToByteArray();

        public static int ReadDummy(ByteArrayReader byteArrayReader) => 
            byteArrayReader.ReadInt();

        public static byte[] WriteWelcome(int clientId, string message) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.Welcome)
                .Write(clientId)
                .Write(message)
                .InsertSize()
                .ToByteArray();

        public static Tuple<int, string> ReadWelcome(ByteArrayReader byteArrayReader) =>
            new Tuple<int, string>(byteArrayReader.ReadInt(), byteArrayReader.ReadString());
        

        public static byte[] WriteWelcomeAck(int clientId, string username) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.WelcomeAck)
                .Write(clientId)
                .Write(username)
                .InsertSize()
                .ToByteArray();
        
        public static Tuple<int, string> ReadWelcomeAck(ByteArrayReader byteArrayReader) =>
            new Tuple<int, string>(byteArrayReader.ReadInt(), byteArrayReader.ReadString());
    }
}