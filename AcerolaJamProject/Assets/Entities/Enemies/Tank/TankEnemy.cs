using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class TankEnemy : MobileEnemy
    {
        [Header("Rushdown")]
        [SerializeField] private float _maxSpeed = 10;
        [SerializeField] private float _speed = 100;
        [SerializeField] private float _stopDistance = 5;
        [Space(3)]
        [SerializeField] private float _shootCooldown;
        private float _shootTimer; 


        [Space]
        [Header("Chaingun")]
        [SerializeField] private GameObject _pellet;
        [SerializeField] private Transform _shootPos;
        private bool _shooting; 
        [SerializeField] private float _shootInterval = 0.1f;
        private float _shootIntervalTimer; 
        [SerializeField] private float _maxShootTime = 3;
        private float _shootingFor; 


        public UnityEvent onShoot;
        public UnityEvent<string, float> onMove; // stupid, but easiest way to hook up the animator

        protected override void Update()
        {
            transform.LookAt(target.transform);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

            if (_shooting)
            {
                _shootingFor += Time.deltaTime;
                _shootIntervalTimer += Time.deltaTime;
                if (_shootIntervalTimer >= _shootInterval)
                {
                    Shoot();
                    _shootIntervalTimer = 0;
                }
                if (_shootingFor >= _maxShootTime)
                {
                    _shootTimer = 0;
                    _shooting = false;
                }
            }
            //_shootPos.LookAt(target.transform);
            base.Update();
        }

        protected override void FixedUpdate()
        {
            if ((target.transform.position - transform.position).magnitude < _stopDistance)
            {
                Move();
            }

            if (_shootTimer < _shootCooldown && ((target.transform.position - transform.position).magnitude > _stopDistance))
            {
                _shootTimer += Time.fixedDeltaTime;
            }
            if (_shootTimer >= _shootCooldown && !_shooting)
            {
                StartShooting();
            }

            base.FixedUpdate();
        }

        private void Move()
        {
            if (_rb.velocity.magnitude > _maxSpeed)
                return;
            //if (!IsGrounded())
            //    return;

            Vector3 dir = transform.position - target.transform.position;

            if (dir.magnitude >= _stopDistance)
                return;

            dir.y = 0;
            dir.Normalize();

            _rb.AddForce(dir * _speed);

            onMove.Invoke("Speed", _rb.velocity.sqrMagnitude);
        }

        private void StartShooting()
        {
            _shootIntervalTimer = 0;
            _shootingFor = 0;
            _shooting = true;
        }
        private void Shoot()
        {
            onShoot.Invoke();
            _shootTimer = 0;
            Instantiate(_pellet, _shootPos.position, _shootPos.rotation);
        }
    }
}