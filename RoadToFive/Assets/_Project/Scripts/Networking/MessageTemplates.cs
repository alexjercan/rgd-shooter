using System;
using _Project.Scripts.ByteArray;
using UnityEngine;

namespace _Project.Scripts.Networking
{
    public static class MessageTemplates
    {
        public static byte[] WriteByteMessage(MessageType messageType, int clientId, byte[] bytes) =>
            new ByteArrayBuilder()
                .Write((int) messageType)
                .Write(clientId)
                .Write(bytes)
                .InsertSize()
                .ToByteArray();
        
        public static Tuple<int, byte[]> ReadByteMessage(ByteArrayReader byteArrayReader) =>
            new Tuple<int, byte[]>(byteArrayReader.ReadInt(), byteArrayReader.ReadBytes(byteArrayReader.UnreadBytes));
        
        public static byte[] WriteDummy() =>
            new ByteArrayBuilder()
                .Write(0)
                .InsertSize()
                .ToByteArray();

        public static byte[] WriteWelcome(int clientId) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.Welcome)
                .Write(clientId)
                .InsertSize()
                .ToByteArray();

        public static int ReadWelcome(ByteArrayReader byteArrayReader) => byteArrayReader.ReadInt();
        

        public static byte[] WriteWelcomeAck(int clientId, string username) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.WelcomeAck)
                .Write(clientId)
                .Write(username)
                .InsertSize()
                .ToByteArray();
        
        public static Tuple<int, string> ReadWelcomeAck(ByteArrayReader byteArrayReader) =>
            new Tuple<int, string>(byteArrayReader.ReadInt(), byteArrayReader.ReadString());

        public static byte[] WriteSpawnPlayer(int playerId, Vector3 position, Vector2 rotation) => 
            new ByteArrayBuilder()
                .Write((int) MessageType.SpawnPlayer)
                .Write(playerId)
                .Write(position)
                .Write(rotation)
                .InsertSize()
                .ToByteArray();
        
        public static Tuple<int, Vector3, Vector2> ReadSpawnPlayer(ByteArrayReader byteArrayReader) => 
            new Tuple<int, Vector3, Vector2>(byteArrayReader.ReadInt(), byteArrayReader.ReadVector3(), byteArrayReader.ReadVector2());
       
        public static byte[] WritePlayerInput(int playerId, Vector3 movementInput, Vector2 rotation) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.PlayerInput)
                .Write(playerId)
                .Write(movementInput)
                .Write(rotation)
                .InsertSize()
                .ToByteArray();
        
        public static Tuple<int, Vector3, Vector2> ReadPlayerInput(ByteArrayReader byteArrayReader) => 
            new Tuple<int, Vector3, Vector2>(byteArrayReader.ReadInt(), byteArrayReader.ReadVector3(), byteArrayReader.ReadVector2());

        public static byte[] WritePlayerMovement(int playerId, Vector3 position, Vector2 rotation) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.PlayerMovement)
                .Write(playerId)
                .Write(position)
                .Write(rotation)
                .InsertSize()
                .ToByteArray();

        public static Tuple<int, Vector3, Vector2> ReadPlayerMovement(ByteArrayReader byteArrayReader) =>
            new Tuple<int, Vector3, Vector2>(byteArrayReader.ReadInt(), byteArrayReader.ReadVector3(), byteArrayReader.ReadVector2());

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