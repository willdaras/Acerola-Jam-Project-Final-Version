using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

namespace Level.Targeting
{
    [System.Serializable]
    public class Patrol
    {
        public BaseEnemy enemy;
        public List<GameObject> patrolPoints;

        private int _index = 0;

        public Patrol()
        {

        }
        public Patrol(GameObject enemy)
        {
            this.enemy = enemy.GetComponent<BaseEnemy>();
        }
        public Patrol(GameObject enemy, IEnumerable<GameObject> points)
        {
            this.enemy = enemy.GetComponent<BaseEnemy>();
            patrolPoints = new List<GameObject>(points);
        }

        public void UpdatePatrol()
        {
            enemy.target = patrolPoints[_index];
            if (Vector3.Distance(enemy.transform.position, patrolPoints[_index].transform.position) < 0.2)
            {
                _index += 1;
                if (_index >= patrolPoints.Count)
                {
                    _index = 0;
                }
                enemy.target = patrolPoints[_index];
            }
        }
    }
}