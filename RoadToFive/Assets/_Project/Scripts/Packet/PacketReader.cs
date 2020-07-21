using System;
using UnityEngine;

namespace _Project.Scripts.Packet
{
    public class PacketReader
    {
        private readonly byte[] _data;
        private int _iterator;
        
        public PacketReader(byte[] data)
        {
            _data = data;
            _iterator = 0;
        }

        public bool ReadBool()
        {
            var result = BitConverter.ToBoolean(_data, _iterator);
            _iterator += sizeof(bool);
            return result;
        }
        
        public int ReadInt()
        {
            var result = BitConverter.ToInt32(_data, _iterator);
            _iterator += sizeof(int);
            return result;
        }

        public float ReadFloat()
        {
            var result = BitConverter.ToSingle(_data, _iterator);
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