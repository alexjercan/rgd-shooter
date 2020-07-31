using _Project.Scripts.ClientSide.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.ClientSide.UserInterface
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject startMenu;
        [SerializeField] private GameObject connectMenu;
        [SerializeField] private InputField ipField;

        private void Awake()
        {
            startMenu.SetActive(true);
            connectMenu.SetActive(false);
        }
        
        public void ConnectToServer()
        {
            startMenu.SetActive(false);
            ipField.interactable = false;
            Client.ConnectToServer(ipField.text);
        }
    }
}
