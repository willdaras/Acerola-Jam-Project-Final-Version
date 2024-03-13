using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Combat;

namespace Weapons
{
    public class SuperShotgun : BaseAttachment
    {
        [SerializeField] private Transform _shootPoint;

        [SerializeField] private int _pelletCount = 15;
        [SerializeField] private GameObject _pellet;
        [SerializeField] private float _spread = 1;
        [SerializeField] private Transform _cam;

        public UnityEvent onStartShoot;
        public UnityEvent onEndShoot;

        private void Shoot()
        {
            for (int i = 0; i < _pelletCount; i++)
            {
                Vector3 direction = ((_cam.transform.position + _cam.transform.forward * 50) - _shootPoint.position).normalized;
                Vector3 spread = _cam.transform.up * Random.Range(-_spread, _spread);
                spread += _cam.transform.right * Random.Range(-_spread, _spread);

                direction += spread.normalized * Random.Range(0f, 0.2f);

                Instantiate(_pellet, _shootPoint.position, Quaternion.LookRotation(direction));
            }
        }

        public override void Attack()
        {
            onStartShoot.Invoke();
            Shoot();
        }

        public override void EndAttack()
        {
            onEndShoot.Invoke();
        }
    }
}