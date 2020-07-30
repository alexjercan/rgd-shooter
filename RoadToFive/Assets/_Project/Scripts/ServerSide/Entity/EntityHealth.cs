using UnityEngine;

namespace _Project.Scripts.ServerSide.Entity
{
    public class EntityHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int maxArmor = 100;

        private int _health;
        private int _armor;

        public bool isDead;

        private void Start() => _health = maxHealth;
        
        public void Damage(int damage)
        {
            _armor -= damage;
            if (_armor >= 0) return;
            
            _health += _armor;
            _armor = 0;
            if (_health > 0) return;
            
            isDead = true;
            _health = 0;
        }

        public void Heal(int heal)
        {
            _health += heal;
            if (_health > maxHealth) _health = maxHealth;
        }

        public void AddArmor(int armorGained)
        {
            _armor += armorGained;
            if (_armor > maxArmor) _armor = maxArmor;
        }
    }
}
