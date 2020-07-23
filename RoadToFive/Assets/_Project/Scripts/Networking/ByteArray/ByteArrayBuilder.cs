using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace _Project.Scripts.Networking.ByteArray
{
    public class ByteArrayBuilder
    {
        private readonly List<byte> _bytes = new List<byte>();

        private ByteArrayBuilder Insert(int data)
        {
            _bytes.InsertRange(0, BitConverter.GetBytes(data));
            return this;
        }
        
        public ByteArrayBuilder InsertSize()
        {
            return Insert(_bytes.Count);
        }

        public ByteArrayBuilder Write(byte data)
        {
            _bytes.Add(data);
            return this;
        }
        
        public ByteArrayBuilder Write(byte[] data)
        {
            _bytes.AddRange(data);
            return this;
        }
        
        public ByteArrayBuilder Write(char data)
        {
            var bytes = BitConverter.GetBytes(data);
            return Write(bytes);
        }

        public ByteArrayBuilder Write(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            var size = bytes.Length;
            return Write(size).Write(bytes);
        }
        
        public ByteArrayBuilder Write(bool data)
        {
            var bytes = BitConverter.GetBytes(data);
            return Write(bytes);
        }

        public ByteArrayBuilder Write(int data)
        {
            var bytes = BitConverter.GetBytes(data);
            return Write(bytes);
        }
        
        public ByteArrayBuilder Write(float data)
        {
            var bytes = BitConverter.GetBytes(data);
            return Write(bytes);
        }

        public ByteArrayBuilder Write(Vector2 data)
        {
            return Write(data.x).Write(data.y);
        }
        
        public ByteArrayBuilder Write(Vector3 data)
        {
            return Write(data.x).Write(data.y).Write(data.z);
        }

        public ByteArrayBuilder Write(Quaternion data)
        {
            return Write(data.x).Write(data.y).Write(data.z).Write(data.w);
        }

        public byte[] ToByteArray()
        {
            return _bytes.ToArray();
        }
    }
}