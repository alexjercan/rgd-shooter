using _Project.Scripts.ClientSide.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UserInterface
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject startMenu;
        [SerializeField] private InputField ipField;

    
        public void ConnectToServer()
        {
            startMenu.SetActive(false);
            ipField.interactable = false;
            Client.ConnectToServer(ipField.text);
        }
    }
}
