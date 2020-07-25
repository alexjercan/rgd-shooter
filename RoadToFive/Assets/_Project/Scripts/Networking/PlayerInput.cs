﻿﻿using System.Numerics;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking
{
    public class PlayerInput
    {
        public int Id { get; }
        public Vector3 MovementInput { get; }
        public Vector2 Rotation { get; set; }

        public PlayerInput(int id, Vector3 movementInput, Vector2 rotation)
        {
            Id = id;
            MovementInput = movementInput;
            Rotation = rotation;
        }

        public static byte[] Serialize(PlayerInput playerData)
        {
            return new ByteArrayBuilder()
                .Write(playerData.Id)
                .Write(playerData.MovementInput)
                .Write(playerData.Rotation)
                .ToByteArray();
        }

        public static PlayerInput Deserialize(ByteArrayReader reader) =>
            new PlayerInput(reader.ReadInt(),
                reader.ReadVector3(),
                reader.ReadVector2());
    }
}