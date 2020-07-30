﻿using System;
using UnityEngine;

namespace _Project.Scripts.Mechanics
{
    public class EntityHealth : MonoBehaviour
    {
        public event EventHandler<EntityHealth> Damaged;
        public event EventHandler Died;

        public int Health { get; private set; }

        [SerializeField] private int maxHealth = 100;

        private void Start() => Health = maxHealth;
        
        public void Damage(int damage)
        {
            if (Health <= 0) return;
            
            Health -= damage;
            Damaged?.Invoke(this, this);
            if (Health > 0) return;
            
            Died?.Invoke(this, EventArgs.Empty);
            Health = 0;
        }

        public void Heal(int heal)
        {
            Health += heal;
            if (Health > maxHealth) Health = maxHealth;
        }
    }
}
