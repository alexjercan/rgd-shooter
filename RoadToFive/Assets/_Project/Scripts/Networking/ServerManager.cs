using System;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.TCP;
using _Project.Scripts.Networking.UDP;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class ServerManager : MonoBehaviour
    {
        private IServerManager _udpServerManager;
        private IServerManager _tcpServerManager;

        private void Awake()
        {
            if (GetComponents<ServerManager>().Length > 1) 
                Logger.Error("Multiple ServerManager instances in the scene!");
        }
        
        private void Start()
        {
            _udpServerManager = new UdpServerManager(ReadMessage);
            _tcpServerManager = new TcpServerManager(ReadMessage);
        }
        
        private void ReadMessage(ByteArrayReader receivedMessage)
        {
            var packetType = (MessageType)receivedMessage.ReadInt();

            switch (packetType)
            {
                case MessageType.Invalid:
                    break;
                case MessageType.Dummy:
                    break;
                case MessageType.DummyAck:
                    break;
                case MessageType.Welcome:
                    break;
                case MessageType.WelcomeAck:
                    HandleWelcomeAck(receivedMessage);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeAck(ByteArrayReader byteArrayReader)
        {
            var (clientId, clientUsername) = MessageTemplates.ReadWelcomeAck(byteArrayReader);
            
            Logger.Info($": received username: '{clientUsername}' of client {clientId}");
            
            //TODO: SPAWN PLAYER
        }
    }
}