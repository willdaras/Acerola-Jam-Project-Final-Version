using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

namespace Weapons
{
    public class ShotgunPellet : MonoBehaviour
    {
        private Rigidbody _rb;

        [SerializeField] private float _despawnTime = 1;
        private float _despawnTimer;

        [SerializeField] private int _damage = 3;

        [SerializeField] private float _speed = 20;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(transform.position + (transform.forward * _speed * Time.fixedDeltaTime));

            _despawnTimer += Time.fixedDeltaTime;
            if (_despawnTimer >= _despawnTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Common.MonoSingleton<Player.Player>.instance.gameObject && _despawnTimer < 0.05f) // TODO make this nicer, its discusting
                return;

            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(_damage);
            }

            Destroy(gameObject);
        }
    }
}