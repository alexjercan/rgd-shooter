﻿using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public class Player
    {
        public int Id;
        public string Username;

        public Vector3 Position;
        public Quaternion Rotation;

        public Player(int id, string username, Vector3 spawnPosition)
        {
            Id = id;
            Username = username;
            Position = spawnPosition;
            Rotation = Quaternion.identity;
        }
    }
}