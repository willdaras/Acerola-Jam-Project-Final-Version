using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class RushdownEnemy : MobileEnemy
    {
        [Header("Rushdown")]
        [SerializeField] private float _maxSpeed = 10;
        [SerializeField] private float _speed = 100;
        [SerializeField] private float _stopDistance = 5;
        [Space(3)]
        [SerializeField] private float _shootCooldown;
        private float _shootTimer;


        [Space]
        [Header("Shotgun")]
        [SerializeField] private int _pelletCount = 15;
        [SerializeField] private GameObject _pellet;
        [SerializeField] private float _spread = 1;
        [SerializeField] private Transform _shootPos;
        [SerializeField] private Transform _cam;

        public UnityEvent onShoot;
        public UnityEvent<string, float> onMove; // stupid, but easiest way to hook up the animator

        protected override void Update()
        {
            transform.LookAt(target.transform);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            base.Update();
        }

        protected override void FixedUpdate()
        {
            Move();

            if (_shootTimer < _shootCooldown && ((target.transform.position - transform.position).magnitude <= _stopDistance))
            {
                _shootTimer += Time.fixedDeltaTime;
            }
            if (_shootTimer >= _shootCooldown)
            {
                Shoot();
            }

            base.FixedUpdate();
        }

        private void Move()
        {
            if (_rb.velocity.magnitude > _maxSpeed)
                return;
            if (!IsGrounded())
                return;

            Vector3 dir = target.transform.position - transform.position;

            if (dir.magnitude <= _stopDistance)
                return;

            dir.y = 0;
            dir.Normalize();

            _rb.AddForce(dir * _speed);

            onMove.Invoke("Speed", _rb.velocity.sqrMagnitude);
        }

        private void Shoot()
        {
            onShoot.Invoke();
            _shootTimer = 0;
            for (int i = 0; i < _pelletCount; i++)
            {
                Vector3 direction = (target.transform.position - _shootPos.position).normalized;
                Vector3 spread = _cam.transform.up * Random.Range(-_spread, _spread);
                spread += _cam.transform.right * Random.Range(-_spread, _spread);

                direction += spread.normalized * Random.Range(0f, 0.2f);

                Instantiate(_pellet, _shootPos.position, Quaternion.LookRotation(direction));
            }
        }

        protected override void OnHealthZero()
        {
            base.OnHealthZero();
        }
    }
}