using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public string mainSceneClient = "MainSceneClient";
        public GameObject mainMenuCanvas;
        public GameObject loadingScreenCanvas;

        private AsyncOperation _mainSceneLoading;
    
        public void PlayGame() 
        {
            mainMenuCanvas.SetActive(false);
            loadingScreenCanvas.SetActive(true);
            _mainSceneLoading = SceneManager.LoadSceneAsync(mainSceneClient, LoadSceneMode.Single);
            _mainSceneLoading.completed += operation =>
            {

            };
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
