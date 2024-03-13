using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Combat;
using Common;

namespace Player
{
    public class PlayerSword : MonoBehaviour
    {
        [SerializeField] private float _attackCooldown = 0.4f;
        private float _attackTimer;
        public bool canAttack => _attackTimer >= _attackCooldown;

        [SerializeField] private float _attackingTime = 0.3f;
        private float _attackingTimer;

        [SerializeField] private int _damage = 20;
        [SerializeField] private float _hitstopTime = 0.1f;
        [SerializeField] private float _screenShakeTime = 0.1f;
        [SerializeField] private float _screenShakeIntensity = 1.5f;
        [SerializeField] private float _screenShakeFrequency = 1;

        public bool attacking => _attackingTimer < _attackingTime;

        public UnityEvent onAttack;
        public UnityEvent onHit;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (_attackTimer < _attackCooldown)
            {
                _attackTimer += Time.deltaTime;
            }
            if (_attackingTimer < _attackingTime)
            {
                _attackingTimer += Time.deltaTime;
                if (_attackingTimer >= _attackingTime)
                    MonoSingleton<Player>.instance.actionable = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                if (other.gameObject == MonoSingleton<Player>.instance.gameObject)
                    return;
                Hit(damageable);
            }
        }

        private void Hit(IDamageable damageable)
        {
            if (!attacking)
                return;
            damageable.Damage(_damage);
            MonoSingleton<GameFeel.HitstopManager>.instance.HitstopFor(_hitstopTime);
            MonoSingleton<GameFeel.ScreenshakeManager>.instance.ScreenshakeFor(_screenShakeIntensity, _screenShakeFrequency, _screenShakeTime);
            onHit.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started && canAttack && MonoSingleton<Player>.instance.actionable)
            {
                onAttack.Invoke();
                _attackTimer = 0;
                _attackingTimer = 0;
                MonoSingleton<Player>.instance.actionable = false;
            }
        }
    }
}
