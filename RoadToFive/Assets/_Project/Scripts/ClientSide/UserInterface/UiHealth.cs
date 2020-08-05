using _Project.Scripts.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.ClientSide.UserInterface
{
    public class UiHealth : MonoBehaviour
    {
        [SerializeField] private Image healthIndicator;
        [SerializeField] private EntityHealth entityHealth;

        public void DamageReceive()
        {
            ChangeIndicatorColor();
        }

        public void HealReceive()
        {
            ChangeIndicatorColor();
        }

        private void ChangeIndicatorColor()
        {
            healthIndicator.color = new Color(1, 0, 0, 1 - entityHealth.Health / (float) entityHealth.MaxHealth);
        }
    }
}