using _Project.Scripts.ClientSide.Networking;
using UnityEngine;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    /// <summary>
    /// PLAYER MANAGER CONTINE TOATE COMPONENTELE CARE DEFINESC JUCATORUL LOCAL
    /// </summary>
    public class LocalPlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private PlayerMovementInput playerMovementInput;
        
        private void FixedUpdate()
        {
            ClientSend.PlayerMovement(playerMovementInput.GetMovementInput(),  playerManager.GetRotation());
        }
    }
}