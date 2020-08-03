using UnityEngine;

namespace _Project.Scripts.ClientSide.UserInterface
{
    public class UiHealth : MonoBehaviour
    {
        public void DamageReceive()
        {
            Debug.Log("Make the screen red. Took Damage!");
        }

        public void HealReceive()
        {
            Debug.Log("Show particles for healing");
        }
    }
}