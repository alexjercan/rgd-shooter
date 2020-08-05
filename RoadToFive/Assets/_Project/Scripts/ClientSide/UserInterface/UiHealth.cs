using _Project.Scripts.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.ClientSide.UserInterface
{
    public class UiHealth : MonoBehaviour
    {
        [SerializeField] private Image healthIndicator;

        public void DamageReceive(EntityHealth entityHealth)
        {
            ChangeIndicatorColor(entityHealth);
        }

        public void HealReceive(EntityHealth entityHealth)
        {
            ChangeIndicatorColor(entityHealth);
        }

        private void ChangeIndicatorColor(EntityHealth entityHealth)
        {
            healthIndicator.color = new Color(1, 0, 0, 1 - entityHealth.Health / (float) entityHealth.MaxHealth);
        }
    }
}