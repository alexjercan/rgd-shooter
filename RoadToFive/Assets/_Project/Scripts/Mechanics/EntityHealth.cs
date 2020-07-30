using System;
using UnityEngine;

namespace _Project.Scripts.Mechanics
{
    public class EntityHealth : MonoBehaviour
    {
        public event EventHandler<EntityHealth> Damaged;

        [SerializeField] private int maxHealth = 100;

        private int _health;

        private void Start() => _health = maxHealth;
        
        public void Damage(int damage)
        {
            if (_health <= 0) return;
            
            _health -= damage;
            Damaged?.Invoke(this, this);
            if (_health > 0) return;
            
            _health = 0;
        }

        public void Heal(int heal)
        {
            _health += heal;
            if (_health > maxHealth) _health = maxHealth;
        }
    }
}
