using _Project.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UserInterface
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject startMenu;
        [SerializeField] private InputField ipField;

        private ClientNetworkInterface _clientNetworkInterface;
    
        private void Awake()
        {
            _clientNetworkInterface = FindObjectOfType<ClientNetworkInterface>();
        }
    
        public void ConnectToServer()
        {
            startMenu.SetActive(false);
            ipField.interactable = false;
            _clientNetworkInterface.ConnectToServer(ipField.text.ToString());
        }
    }
}
