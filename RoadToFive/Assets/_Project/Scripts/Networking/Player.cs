using System.Numerics;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking
{
    public class Player
    {
        private int _id;
        private string _username;

        private Vector3 _position;
        private Quaternion _rotation;

        public string Name => _username;
        
        public Player(int id, string username, Vector3 position, Quaternion rotation)
        {
            _id = id;
            _username = username;
            _position = position;
            _rotation = rotation;
        }

        public static byte[] Serialize(Player player)
        {
            return new ByteArrayBuilder()
                .Write(player._id)
                .Write(player._username)
                .Write(player._position)
                .Write(player._rotation)
                .ToByteArray();
        }

        public static Player Deserialize(ByteArrayReader reader) =>
            new Player(reader.ReadInt(),
                reader.ReadString(),
                reader.ReadVector3(),
                reader.ReadQuaternion());
    }
}