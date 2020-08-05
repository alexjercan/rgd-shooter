using _Project.Scripts.ClientSide.Player;
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

        public void AmmoIndicatorUpdate(PlayerInventory inventory)
        {
            ammoIndicator.text = $"{inventory.Ammo}";
        }
    }
}