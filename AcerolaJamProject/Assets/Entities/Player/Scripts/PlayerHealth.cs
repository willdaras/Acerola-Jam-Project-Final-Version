using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Combat;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private int _maxHealth = 100;
        [ContextMenuItem("Debug Damage", "DebugDamage")]
        [SerializeField] private int _health;
        public int Health => _health;

        [SerializeField] private bool _invincible;
        public bool Invincible => _invincible;

        [SerializeField] private float _invincibleTime = 0.5f;

        public UnityEvent onHealthZero;

        private void Start()
        {
            _health = _maxHealth;
        }

        public void BecomeInvincible()
        {
            _invincible = true;
        }
        public void BecomeInvincible(float time)
        {
            StartCoroutine(InvincibleFor(time));
        }
        public void DisableInvincibility()
        {
            _invincible = false;
        }

        private void DebugDamage()
        {
            Damage(10);
        }

        public void Damage(int amount)
        {
            if (amount <= 0)
            {
                _health -= amount;
                _health = Mathf.Clamp(Health, 0, _maxHealth);
                return;
            }
            if (Invincible)
                return;
            _health -= amount;
            BecomeInvincible(_invincibleTime);
            _health = Mathf.Clamp(Health, 0, _maxHealth);
            if (Health <= 0)
            {
                onHealthZero.Invoke();
            }
        }


        private IEnumerator InvincibleFor(float time)
        {
            if (time <= 0)
                yield break;
            _invincible = true;
            yield return new WaitForSeconds(time);
            _invincible = false;
        }
    }
}