using UnityEngine;

namespace _Project.Scripts.Networking
{
    public class NetworkManagerMock : MonoBehaviour
    {
        [SerializeField] private ClientInputHandler clientInputHandler;
        [SerializeField] private Transform clientCharacterTransform;

        [SerializeField] private ServerInputHandler serverInputHandler;
        [SerializeField] private Transform serverCharacterTransform;


        private void Update()
        {
            //SIMULEZ TRANSMITEREA DE DATE DE LA CLIENT LA SERVER PENTRU UN SINGUR JUCATOR
            serverInputHandler.MovementInput = clientInputHandler.MovementInput;
            serverInputHandler.JumpInput = clientInputHandler.JumpInput;
            serverInputHandler.ClientRotationYValue = clientCharacterTransform.localEulerAngles.y;
            
            //SIMULEZ TRANSMITEREA DE DATE DE LA SERVER LA CLIENT PENTRU UN SINGUR JUCATOR
            clientInputHandler.ServerPositionValue = serverCharacterTransform.position;
        }
    }
}