﻿using System.Numerics;
 using _Project.Scripts.ByteArray;

 namespace _Project.Scripts.Networking
{
    public class PlayerData
    {
        public int Id { get; }
        public Vector3 Position { get; set; }
        public Vector2 Rotation { get; set; }

        public PlayerData(int id, Vector3 position, Vector2 rotation)
        {
            Id = id;
            Position = position;
            Rotation = rotation;
        }

        public static byte[] Serialize(PlayerData playerData)
        {
            return new ByteArrayBuilder()
                .Write(playerData.Id)
                .Write(playerData.Position)
                .Write(playerData.Rotation)
                .ToByteArray();
        }

        public static PlayerData Deserialize(ByteArrayReader reader) =>
            new PlayerData(reader.ReadInt(),
                reader.ReadVector3(),
                reader.ReadVector2());
    }
}