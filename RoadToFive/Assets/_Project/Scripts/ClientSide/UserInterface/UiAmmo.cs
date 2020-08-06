using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.ClientSide.UserInterface
{
    public class UiAmmo : MonoBehaviour
    {
        [SerializeField] private Text ammoIndicator;
        [SerializeField] private AudioSource pickUpSound;

        private void Start()
        {
            ammoIndicator.text = "0";
        }

        public void AmmoIndicatorUpdate(int ammo)
        {
            if (ammo - int.Parse(ammoIndicator.text) > 0)
            {
                pickUpSound.Play();
            }
            ammoIndicator.text = $"{ammo}";
            
        }
    }
}