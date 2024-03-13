using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Combat;

namespace Weapons
{
    public class Chaingun : BaseAttachment
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private int _damage = 1;
        [SerializeField] private LayerMask _hitLayers;

        [SerializeField] private float _fireRate = 20;
        private float _timeBetweenShots => 1 / _fireRate;
        private float _shotTimer;

        [SerializeField] private GameObject _bulletTrail;

        public UnityEvent onStartShoot;
        public UnityEvent onShoot;
        public UnityEvent onEndShoot;

        private void Update()
        {
            if (Attacking)
            {
                if (_shotTimer > _timeBetweenShots)
                {
                    _shotTimer = 0;
                    Shoot();
                }
                else
                {
                    _shotTimer += Time.deltaTime;
                }
            }
        }

        public void Shoot()
        {
            onShoot.Invoke();

            if (Physics.Raycast(_shootPoint.position, _shootPoint.forward, out RaycastHit hit, 100, _hitLayers))
            {
                GameObject target = hit.collider.gameObject;

                if (target.TryGetComponent(out IDamageable damageable))
                {
                    damageable.Damage(_damage);
                }
            }

            GameObject trail = Instantiate(_bulletTrail);
            LineRenderer renderer = trail.GetComponent<LineRenderer>();
            renderer.SetPosition(0, _shootPoint.position);
            renderer.SetPosition(1, _shootPoint.position + (_shootPoint.forward * 50));
        }

        public override void Attack()
        {
            onStartShoot.Invoke();
        }

        public override void EndAttack()
        {
            onEndShoot.Invoke();
        }
    }
}