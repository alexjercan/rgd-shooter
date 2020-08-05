using _Project.Scripts.Mechanics;
using System;
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

        public void DamageReceive(EntityHealth entityHealth)
        {
            ChangeIndicatorColor(entityHealth);
            StartCoroutine(FlashScreen());
        }

        public void HealReceive(EntityHealth entityHealth)
        {
            ChangeIndicatorColor(entityHealth);
            StartCoroutine(HealingEffect());
        }

        private void ChangeIndicatorColor(EntityHealth entityHealth)
        {
            healthIndicator.color = new Color(1, 0, 0, 1 - entityHealth.Health / (float) entityHealth.MaxHealth);
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