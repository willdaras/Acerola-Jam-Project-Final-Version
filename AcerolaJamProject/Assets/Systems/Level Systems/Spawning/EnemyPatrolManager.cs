using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level.Targeting
{
    public class EnemyPatrolManager : MonoBehaviour
    {
        [SerializeField] private List<Patrol> _patrols;
        [SerializeField] private string _enemyTag = "Player";
        private GameObject _target;
        private bool _targetWithin;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_enemyTag))
            {
                _target = other.gameObject;
                _targetWithin = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_enemyTag))
            {
                _targetWithin = false;
            }
        }

        private void Update()
        {
            foreach (Patrol patrol in _patrols)
            {
                if (!_targetWithin)
                {
                    patrol.UpdatePatrol();
                }
                else
                {
                    patrol.enemy.target = _target;
                }
            }
        }
    }
}
