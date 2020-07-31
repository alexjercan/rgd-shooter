using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace _Project.Scripts.Util.DataStructure
{
    public enum ServerPackets
    {
        Welcome,
        SpawnPlayer,
        PlayerPosition,
        PlayerRotation,
        PlayerDisconnected,
        PlayerHealth,
        CreateItemSpawner,
        ItemSpawned,
        ItemPickedUp,
        AmmoPickedUp,
        WeaponPickedUp,
        HandWeaponUpdate,
        InitializeInventory,
    }

    public enum ClientPackets
    {
        WelcomeReceived,
        PlayerMovement,
        PlayerShoot,
        HandWeapon,
    }
    
    public sealed class Packet : IDisposable //DONE DO NOT MODIFY
    {
        private List<byte> _byteBuffer;
        private byte[] _byteArray;
        private int _readIndex;
        
        public Packet()
        {
            _byteBuffer = new List<byte>();
            _readIndex = 0;
        }

        public Packet(int id)
        {
            _byteBuffer = new List<byte>();
            _readIndex = 0;

            Write(id);
        }

        public Packet(byte[] data)
        {
            _byteBuffer = new List<byte>(); 
            _readIndex = 0; 

            AddBytes(data);
        }
        
        public void AddBytes(byte[] data)
        {
            Write(data);
            _byteArray = _byteBuffer.ToArray();
        }

        public Packet InsertLength()
        {
            _byteBuffer.InsertRange(0, BitConverter.GetBytes(_byteBuffer.Count));
            return this;
        }

        public Packet Insert(int value)
        {
            _byteBuffer.InsertRange(0, BitConverter.GetBytes(value));
            return this;
        }

        public byte[] ToArray()
        {
            _readIndex = 0;
            _byteArray = _byteBuffer.ToArray();
            return _byteArray;
        }

        public int Length() => _byteBuffer.Count;

        public int UnreadLength() => Length() - _readIndex;
        
        public void Reset(bool shouldReset = true)
        {
            if (shouldReset == false)
            {
                _readIndex -= 4;
                return;
            }
            
            _byteBuffer.Clear();
            _byteArray = null;
            _readIndex = 0;
        }
        
        public Packet Write(byte value)
        {
            _byteBuffer.Add(value);
            return this;
        }

        public Packet Write(byte[] value)
        {
            _byteBuffer.AddRange(value);
            return this;
        }

        public Packet Write(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            return Write(bytes);
        }

        public Packet Write(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            return Write(bytes);
        }

        public Packet Write(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            return Write(bytes);
        }
        
        public Packet Write(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            return Write(bytes);
        }

        public Packet Write(bool value)
        {
            var bytes = BitConverter.GetBytes(value);
            return Write(bytes);
        }

        public Packet Write(string value)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            var size = bytes.Length;
            return Write(size).Write(bytes);
        }
        
        public Packet Write(Vector2 value)
        {
            return Write(value.x).Write(value.y);
        }

        public Packet Write(Vector3 value)
        {
            return Write(value.x).Write(value.y).Write(value.z);
        }

        public Packet Write(Quaternion value)
        {
            return Write(value.x).Write(value.y).Write(value.z).Write(value.w);
        }
        
        public byte ReadByte()
        {
            var value = _byteArray[_readIndex];
            _readIndex += sizeof(byte);
            return value;
        }
        
        public byte[] ReadBytes(int length)
        {
            var value = _byteBuffer.GetRange(_readIndex, length).ToArray();
            _readIndex += length;
            return value;
        }

        public short ReadShort()
        {
            var value = BitConverter.ToInt16(_byteArray, _readIndex);
            _readIndex += sizeof(short);
            return value;
        }

        public int ReadInt()
        {
            var result = BitConverter.ToInt32(_byteArray, _readIndex);
            _readIndex += sizeof(int);
            return result;
        }


        public long ReadLong()
        {
            var result = BitConverter.ToInt64(_byteArray, _readIndex);
            _readIndex += sizeof(long);
            return result;
        }

        public float ReadFloat()
        {
            var result = BitConverter.ToSingle(_byteArray, _readIndex);
            _readIndex += sizeof(float);
            return result;
        }

        public bool ReadBool()
        {
            var result = BitConverter.ToBoolean(_byteArray, _readIndex);
            _readIndex += sizeof(bool);
            return result;
        }

        public string ReadString()
        {
            var size = ReadInt();
            var result = Encoding.ASCII.GetString(ReadBytes(size));
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

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _byteBuffer = null;
                _byteArray = null;
                _readIndex = 0;
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}