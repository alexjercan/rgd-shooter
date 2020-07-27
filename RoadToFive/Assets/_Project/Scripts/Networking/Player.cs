using UnityEngine;

namespace _Project.Scripts.Networking
{
    public class Player
    {
        public int Id { get; set; }
        public string Username { get; set; }
        
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public Player(int id, string username, Vector3 position)
        {
            Id = id;
            Username = username;
            Position = position;
            Rotation = Quaternion.identity;
        }
    }
}