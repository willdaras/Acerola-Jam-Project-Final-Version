using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Weapons
{
    public class RocketLauncher : BaseAttachment
    {
        [SerializeField] private GameObject _rocket;
        [SerializeField] private Transform _spawnPos;

        public UnityEvent onShoot;

        public override void Attack()
        {
            Instantiate(_rocket, _spawnPos.position, _spawnPos.rotation);
        }

        public override void EndAttack()
        {
            return;
        }
    }
}