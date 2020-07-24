using System;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.TCP;
using _Project.Scripts.Networking.UDP;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class ClientManager : MonoBehaviour
    {
        private IClientManager _udpClientManager;
        private IClientManager _tcpClientManager;
        
        private void Awake()
        {
            if (GetComponents<ClientManager>().Length > 1) 
                Logger.Error("Multiple ClientManager instances in the scene!");
        }

        private void Start()
        {
            _udpClientManager = new UdpClientManager(ReadMessage);
        }
        
        private void ReadMessage(ByteArrayReader receiveMessage)
        {
            var packetType = (MessageType)receiveMessage.ReadInt();

            switch (packetType)
            {
                case MessageType.Invalid:
                    break;
                case MessageType.Dummy:
                    HandleDummy(receiveMessage);
                    break;
                case MessageType.DummyAck:
                    break;
                case MessageType.Welcome:
                    HandleWelcome(receiveMessage);
                    break;
                case MessageType.WelcomeAck:
                    break;
                default:
                    return;
            }
        }

        private void HandleDummy(ByteArrayReader byteArrayReader)
        {
            var clientId = MessageTemplates.ReadDummy(byteArrayReader);
            Logger.Info($": received my new client id '{clientId}' from server!");
            _udpClientManager.SetClientId(clientId);
            _tcpClientManager = new TcpClientManager(ReadMessage);
        }
        
        private void HandleWelcome(ByteArrayReader byteArrayReader)
        {
            var (clientId, message) = MessageTemplates.ReadWelcome(byteArrayReader);

            Logger.Info($": received '{message}' from server!");
            
            _tcpClientManager.SendMessage(MessageTemplates.WriteWelcomeAck(clientId, "guest " + clientId));
        }
    }
}