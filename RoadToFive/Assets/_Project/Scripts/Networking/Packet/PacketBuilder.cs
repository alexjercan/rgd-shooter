using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace _Project.Scripts.Networking.Packet
{
    public class PacketBuilder
    {
        private readonly List<byte> _bytes = new List<byte>();
        
        public PacketBuilder InsertSize()
        {
            _bytes.InsertRange(0, BitConverter.GetBytes(_bytes.Count));
            return this;
        }

        public PacketBuilder Write(byte data)
        {
            _bytes.Add(data);
            return this;
        }
        
        public PacketBuilder Write(byte[] data)
        {
            _bytes.AddRange(data);
            return this;
        }
        
        public PacketBuilder Write(char data)
        {
            var bytes = BitConverter.GetBytes(data);
            _bytes.AddRange(bytes);
            return this;
        }

        public PacketBuilder Write(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            _bytes.AddRange(bytes);
            return Write(PacketInfo.NullTerminator);
        }
        
        public PacketBuilder Write(bool data)
        {
            var bytes = BitConverter.GetBytes(data);
            _bytes.AddRange(bytes);
            return this;
        }

        public PacketBuilder Write(int data)
        {
            var bytes = BitConverter.GetBytes(data);
            _bytes.AddRange(bytes);
            return this;
        }
        
        public PacketBuilder Write(float data)
        {
            var bytes = BitConverter.GetBytes(data);
            _bytes.AddRange(bytes);
            return this;
        }

        public PacketBuilder Write(Vector2 data)
        {
            return Write(data.x).Write(data.y);
        }
        
        public PacketBuilder Write(Vector3 data)
        {
            return Write(data.x).Write(data.y).Write(data.z);
        }

        public PacketBuilder Write(Quaternion data)
        {
            return Write(data.x).Write(data.y).Write(data.z).Write(data.w);
        }

        public byte[] ToByteArray()
        {
            return _bytes.ToArray();
        }
    }
}