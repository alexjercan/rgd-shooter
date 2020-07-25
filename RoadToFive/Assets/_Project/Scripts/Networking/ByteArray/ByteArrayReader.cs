﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace _Project.Scripts.Networking.ByteArray
{
    public class ByteArrayReader
    {
        private readonly List<byte> _data;
        private int _iterator;

        public int UnreadBytes => _data.Count - _iterator;
        
        public ByteArrayReader()
        {
            _data = new List<byte>();
            _iterator = 0;
        }
        
        public ByteArrayReader(byte[] data)
        {
            _data = new List<byte>(data);
            _iterator = 0;
        }

        public void AddBytes(byte[] data)
        {
            _data.AddRange(data);
        }

        public byte[] ReadBytes(int size)
        {
            var result = _data.GetRange(_iterator, size).ToArray();
            _iterator += size;
            return result;
        }

        public char ReadChar()
        {
            var result = BitConverter.ToChar(_data.ToArray(), _iterator);
            _iterator += sizeof(bool);
            return result;
        }
        
        public string ReadString()
        {
            var size = ReadInt();
            var result = Encoding.ASCII.GetString(ReadBytes(size));
            return result;
        }
        
        public bool ReadBool()
        {
            var result = BitConverter.ToBoolean(_data.ToArray(), _iterator);
            _iterator += sizeof(bool);
            return result;
        }
        
        public int ReadInt()
        {
            var result = BitConverter.ToInt32(_data.ToArray(), _iterator);
            _iterator += sizeof(int);
            return result;
        }

        public float ReadFloat()
        {
            var result = BitConverter.ToSingle(_data.ToArray(), _iterator);
            _iterator += sizeof(float);
            return result;
        }

        public Vector2 ReadVector2()
        {
            return new Vector2(ReadFloat(), ReadFloat());
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
        }

        public Quaternion ReadQuaternion()
        {
            return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
        }
    }
}