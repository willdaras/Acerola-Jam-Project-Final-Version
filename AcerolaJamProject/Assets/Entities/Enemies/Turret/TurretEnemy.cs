using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

namespace Enemies
{
    public class TurretEnemy : BaseEnemy
    {
        private BaseAttachment _weapon;

        [Header("Turret")]
        [SerializeField] private Transform _mainPivot;
        [SerializeField] private Transform _gunPivot;
        [SerializeField] private Transform _eyePivot;

        [Space]
        [SerializeField] private float _attackInterval = 3;
        private float _attackTimer;

        public UnityEvent onAttackStart;
        public UnityEvent onAttackEnd;

        protected override void Start()
        {
            _weapon = GetComponentInChildren<BaseAttachment>();
            base.Start();
        }

        protected override void Update()
        {
            Rotate();

            if (_attackTimer >= _attackInterval)
            {
                StartCoroutine(AttackRoutine());
                _attackTimer = 0;
            }
            else
            {
                _attackTimer += Time.deltaTime;
            }

            base.Update();
        }

        private IEnumerator AttackRoutine()
        {
            onAttackStart.Invoke();
            yield return new WaitForSeconds(0.15f);
            _weapon.Attack();
            onAttackEnd.Invoke();
        }

        private void Rotate()
        {
            _mainPivot.LookAt(target.transform);
            _mainPivot.localEulerAngles = new Vector3(0, _mainPivot.localEulerAngles.y, 0);
            _gunPivot.LookAt(target.transform);
            _gunPivot.localEulerAngles = new Vector3(_gunPivot.localEulerAngles.x, 0, 0);
            _eyePivot.LookAt(target.transform);
            _eyePivot.localEulerAngles = new Vector3(_eyePivot.localEulerAngles.x, 0, 0);
        }
    }
}