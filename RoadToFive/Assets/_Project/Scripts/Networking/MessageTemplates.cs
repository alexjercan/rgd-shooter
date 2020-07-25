using System;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking
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

        public static byte[] WriteSpawnPlayer(PlayerData playerData) => 
            new ByteArrayBuilder()
                .Write((int) MessageType.SpawnPlayer)
                .Write(PlayerData.Serialize(playerData))
                .InsertSize()
                .ToByteArray();
        
        public static PlayerData ReadSpawnPlayer(ByteArrayReader byteArrayReader) => PlayerData.Deserialize(byteArrayReader);
        
        public static byte[] WritePlayerInput(PlayerInput playerInput) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.PlayerInput)
                .Write(PlayerInput.Serialize(playerInput))
                .InsertSize()
                .ToByteArray();
        
        public static PlayerInput ReadPlayerInput(ByteArrayReader byteArrayReader) => PlayerInput.Deserialize(byteArrayReader);

        public static byte[] WritePlayerDisconnect(int clientId) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.PlayerDisconnect)
                .Write(clientId)
                .InsertSize()
                .ToByteArray();

        public static int ReadPlayerDisconnect(ByteArrayReader byteArrayReader) => byteArrayReader.ReadInt();

        public static byte[] WriteServerDisconnect() =>
            new ByteArrayBuilder()
                .Write((int) MessageType.ServerDisconnect)
                .InsertSize()
                .ToByteArray();

        public static void ReadServerDisconnect(ByteArrayReader byteArrayReader) {}
    }
}