using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class SniperEnemy : MobileEnemy
    {
        [Header("Rushdown")]
        [SerializeField] private float _maxSpeed = 10;
        [SerializeField] private float _speed = 100;
        [SerializeField] private float _stopDistance = 5;
        [Space(3)]
        [SerializeField] private float _shootCooldown;
        private float _shootTimer;


        [Space]
        [Header("Sniper")]
        [SerializeField] private GameObject _pellet;
        [SerializeField] private Transform _shootPos;


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
            //if (!IsGrounded())
            //    return;

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
            Instantiate(_pellet, _shootPos.position, _shootPos.rotation);
        }
    }
}