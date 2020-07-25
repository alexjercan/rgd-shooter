using System.Numerics;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking
{
    public class PlayerData
    {
        public int Id { get; }
        public string Username { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public PlayerData(int id, string username, Vector3 position, Quaternion rotation)
        {
            Id = id;
            Username = username;
            Position = position;
            Rotation = rotation;
        }

        public static byte[] Serialize(PlayerData playerData)
        {
            return new ByteArrayBuilder()
                .Write(playerData.Id)
                .Write(playerData.Username)
                .Write(playerData.Position)
                .Write(playerData.Rotation)
                .ToByteArray();
        }

        public static PlayerData Deserialize(ByteArrayReader reader) =>
            new PlayerData(reader.ReadInt(),
                reader.ReadString(),
                reader.ReadVector3(),
                reader.ReadQuaternion());
    }
}