using System;
using UnityEngine;

namespace _Project.Scripts.Mechanics
{
    public class EntityHealth : MonoBehaviour //DONE DO NOT MODIFY
    {
        public event EventHandler<EntityHealth> Damaged;
        public event EventHandler<EntityHealth> Healed;
        public event EventHandler Died;

        public int Health { get; private set; }

        [SerializeField] private int maxHealth = 100;

        private void Start() => Health = maxHealth;

        public void SetHealth(int health)
        {
            Health = health;
            if (Health <= 0) Died?.Invoke(this, EventArgs.Empty);
            if (Health > maxHealth) Health = maxHealth;
        }
        
        public void Damage(int damage)
        {
            if (Health <= 0 || damage < 0) return;
            
            Health -= damage;
            Damaged?.Invoke(this, this);
            if (Health > 0) return;
            
            Died?.Invoke(this, EventArgs.Empty);
            Health = 0;
        }

        public void Heal(int heal)
        {
            Healed?.Invoke(this, this);
            SetHealth(Health + heal);
        }
    }
}
