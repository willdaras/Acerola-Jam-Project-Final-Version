using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Combat;
using Enemies;

namespace Level.Targeting
{
    [System.Serializable]
    public class Wave
    {
        public bool active { get; private set; }

        [SerializeField] private List<SpawnData> _enemies;

        public UnityEvent waveDead;

        private List<Vector3> _positions = new List<Vector3>();

        public void Init()
        {
            foreach (SpawnData data in _enemies)
            {
                _positions.Add(data.enemy.transform.position);
            }
        }

        public void SpawnWave(GameObject target, MonoBehaviour coroutineRunner)
        {
            if (active)
                return;
            coroutineRunner.StartCoroutine(Spawn(target));
        }

        private IEnumerator Spawn(GameObject target)
        {
            foreach (SpawnData spawn in _enemies)
            {
                yield return new WaitForSeconds(spawn.delay);
                spawn.enemy.target = target;
                spawn.enemy.gameObject.SetActive(true);
            }
            active = true;
        }

        public void UpdateWave()
        {
            CheckEnemies();
        }

        public void Reset()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                SpawnData data = _enemies[i];
                data.enemy.transform.position = _positions[i];
                data.enemy.GetComponent<IDamageable>().Damage(-300);
                data.enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                data.enemy.gameObject.SetActive(false);
            }
            active = false;
        }

        private void CheckEnemies()
        {
            foreach (SpawnData data in _enemies)
            {
                if (data.enemy.gameObject.activeInHierarchy)
                    return;
            }
            waveDead.Invoke();
        }

        [System.Serializable]
        private struct SpawnData
        {
            public float delay;
            public BaseEnemy enemy;
        }
    }
}
