using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Combat;

namespace Weapons
{
    public class Rocket : MonoBehaviour
    {
        private Rigidbody _rb;

        [SerializeField] private float _velocity = 15;

        [SerializeField] private float _despawnTime = 2;
        private float _despawnTimer;

        [SerializeField] private float _explosionRadius = 1.5f;
        [SerializeField] private LayerMask _explosionLayers;

        [SerializeField] private int _directDamage = 50;
        [SerializeField] private int _explosionDamage = 20;
        [SerializeField] private float _explosionForce = 20f;

        [SerializeField] private GameObject _explosion;

        public UnityEvent onExplode;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_despawnTimer >= _despawnTime)
            {
                Destroy(gameObject);
            }
            else
            {
                _despawnTimer += Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            _rb.velocity = transform.forward * _velocity;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit(other.gameObject);
        }

        private void OnHit(GameObject hit)
        {
            if (_despawnTimer < 0.05)
                return;

            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(_directDamage);
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _explosionLayers);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddExplosionForce(_explosionForce, transform.position, _explosionLayers);
                }
                if (collider.TryGetComponent(out IDamageable damageable1))
                {
                    damageable1.Damage(_explosionDamage);
                }
            }

            Instantiate(_explosion, transform.position, Quaternion.identity);

            onExplode.Invoke();

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}