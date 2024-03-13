using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Combat;

namespace Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IHealth
    {
        protected Rigidbody _rb;

        public GameObject target;

        [Space]
        [Header("Health")]
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] protected int _health;
        public int Health => _health;

        [SerializeField] private bool _invincible;
        public bool Invincible => _invincible;

        [SerializeField] private float _invincibleTime = 0.1f;

        public UnityEvent onHealthZero;

        protected virtual void Start()
        {
            _health = _maxHealth;
            _rb = GetComponent<Rigidbody>();
        }

        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        #region Health
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

        public void Damage(int amount)
        {
            if (Invincible)
                return;
            _health -= amount;
            BecomeInvincible(_invincibleTime);

            if (Health <= 0)
            {
                OnHealthZero();
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

        protected virtual void OnHealthZero()
        {
            onHealthZero.Invoke();
        }
        #endregion
    }
}