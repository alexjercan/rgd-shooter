using System;
using _Project.Scripts.ByteArray;
using UnityEngine;

namespace _Project.Scripts.Networking
{
    public static class MessageTemplates
    {
        public static byte[] WriteByteMessage(MessageType messageType, byte[] bytes) =>
            new ByteArrayBuilder()
                .Write((int) messageType)
                .Write(bytes)
                .InsertSize()
                .ToByteArray();
        
        public static byte[] ReadByteMessage(ByteArrayReader byteArrayReader) => byteArrayReader.ReadBytes(byteArrayReader.UnreadBytes);
        
        public static byte[] WriteDummy() =>
            new ByteArrayBuilder()
                .Write(0)
                .InsertSize()
                .ToByteArray();

        public static byte[] WriteWelcome() =>
            new ByteArrayBuilder()
                .Write((int) MessageType.Welcome)
                .InsertSize()
                .ToByteArray();

        public static void ReadWelcome(ByteArrayReader byteArrayReader) {}
        

        public static byte[] WriteWelcomeAck(string username) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.WelcomeAck)
                .Write(username)
                .InsertSize()
                .ToByteArray();
        
        public static string ReadWelcomeAck(ByteArrayReader byteArrayReader) => byteArrayReader.ReadString();

        public static byte[] WriteSpawnPlayer(int clientId, Vector3 position, Vector2 rotation) => 
            new ByteArrayBuilder()
                .Write((int) MessageType.SpawnPlayer)
                .Write(clientId)
                .Write(position)
                .Write(rotation)
                .InsertSize()
                .ToByteArray();
        
        public static Tuple<int, Vector3, Vector2> ReadSpawnPlayer(ByteArrayReader byteArrayReader) => 
            new Tuple<int, Vector3, Vector2>(byteArrayReader.ReadInt(), byteArrayReader.ReadVector3(), byteArrayReader.ReadVector2());
       
        public static byte[] WritePlayerInput(Vector3 movementInput, Vector2 rotation) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.PlayerInput)
                .Write(movementInput)
                .Write(rotation)
                .InsertSize()
                .ToByteArray();
        
        public static Tuple<Vector3, Vector2> ReadPlayerInput(ByteArrayReader byteArrayReader) => 
            new Tuple<Vector3, Vector2>(byteArrayReader.ReadVector3(), byteArrayReader.ReadVector2());

        public static byte[] WritePlayerMovement(int clientId, Vector3 position, Vector2 rotation) =>
            new ByteArrayBuilder()
                .Write((int) MessageType.PlayerMovement)
                .Write(clientId)
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