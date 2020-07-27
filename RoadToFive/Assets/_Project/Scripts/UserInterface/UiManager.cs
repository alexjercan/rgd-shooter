using _Project.Scripts.ClientSide.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UserInterface
{
    //UGLY UI
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private string mainSceneName;

        [SerializeField] private GameObject startMenu;
        [SerializeField] private GameObject connectMenu;
        [SerializeField] private InputField ipField;
        [SerializeField] private Image progressBar;

        private AsyncOperation _asyncOperation;
        
        private void Awake()
        {
            startMenu.SetActive(true);
            connectMenu.SetActive(false);
            _asyncOperation = SceneManager.LoadSceneAsync(mainSceneName);
            _asyncOperation.completed += operation =>
            {
                connectMenu.SetActive(true);
                progressBar.gameObject.SetActive(false);
            };
        }

        private void Update()
        {
            progressBar.fillAmount = _asyncOperation.progress;
        }


        public void ConnectToServer()
        {
            startMenu.SetActive(false);
            ipField.interactable = false;
            Client.ConnectToServer(ipField.text);
        }
    }
}
