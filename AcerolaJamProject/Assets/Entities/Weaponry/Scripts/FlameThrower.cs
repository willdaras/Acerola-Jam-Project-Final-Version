using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Combat;

namespace Weapons
{
    public class FlameThrower : BaseAttachment
    {
        [SerializeField] private Transform _flamePoint;
        [SerializeField] private Vector3 _size;
        [SerializeField] private int _damage = 1;

        public UnityEvent onStartShoot;
        public UnityEvent onEndShoot;

        public override void Attack()
        {
            onStartShoot.Invoke();
        }

        public void Update()
        {
            if (Attacking)
            {
                Collider[] colliders = Physics.OverlapBox(_flamePoint.position, _size, Quaternion.LookRotation(_flamePoint.forward));
                Debug.Log(colliders[0]);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.Damage(_damage);
                    }
                }
            }
        }
        public override void EndAttack()
        {
            onEndShoot.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(_flamePoint.position, _size * 2);
        }
    }
}