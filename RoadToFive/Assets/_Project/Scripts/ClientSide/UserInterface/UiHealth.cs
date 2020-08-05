using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.ClientSide.UserInterface
{
    public class UiHealth : MonoBehaviour
    {
        [SerializeField] private Image healthIndicator;
        [SerializeField] private ParticleSystem healingParticles;
        [SerializeField] private Image damageFlash;

        public void DamageReceive(int health, int maxHealth)
        {
            ChangeIndicatorColor(health, maxHealth);
            StartCoroutine(FlashScreen());
        }

        public void HealReceive(int health, int maxHealth)
        {
            ChangeIndicatorColor(health, maxHealth);
            StartCoroutine(HealingEffect());
        }

        private void ChangeIndicatorColor(int health, int maxHealth)
        {
            healthIndicator.color = new Color(1, 0, 0, 1 - health / (float) maxHealth);
        }

        private IEnumerator FlashScreen()
        {
            damageFlash.gameObject.SetActive(true);
            while (damageFlash.color.a < 0.85f)
            {
                damageFlash.color = Color.Lerp(damageFlash.color, new Color(1, 0, 0, 1), 0.3f);
                yield return null;
            }
            while (damageFlash.color.a > 0.05f)
            {
                damageFlash.color = Color.Lerp(damageFlash.color, new Color(1, 0, 0, 0), 0.3f);
                yield return null;
            }
            damageFlash.gameObject.SetActive(false);
        }

        private IEnumerator HealingEffect()
        {
            healingParticles.gameObject.SetActive(true);
            healingParticles.Play();
            yield return new WaitForSeconds(2);
            healingParticles.Stop();
            healingParticles.gameObject.SetActive(false);
        }
    }
}