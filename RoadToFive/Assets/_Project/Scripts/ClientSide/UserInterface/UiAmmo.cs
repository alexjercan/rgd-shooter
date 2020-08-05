using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.ClientSide.UserInterface
{
    public class UiAmmo : MonoBehaviour
    {
        [SerializeField] private Text ammoIndicator;

        private void Start()
        {
            ammoIndicator.text = "0";
        }

        public void AmmoIndicatorUpdate(int ammo)
        {
            ammoIndicator.text = $"{ammo}";
        }
    }
}