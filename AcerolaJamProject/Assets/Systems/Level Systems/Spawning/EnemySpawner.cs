using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level.Targeting
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Wave> _waves;

        [SerializeField] private float _timeBetweenUpdates = 0.5f;
        private float _timer;

        [SerializeField] private string _enemyTag = "Player";
        private GameObject _target;

        private bool _triggered;

        private void Start()
        {
            foreach (Wave wave in _waves)
            {
                wave.Init();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_enemyTag))
            {
                if (_triggered)
                    return;
                _triggered = true;

                _target = other.gameObject;
                SpawnWave(0);
            }
        }

        public void SpawnWave(int index)
        {
            index = Mathf.Clamp(index, 0, _waves.Count);
            _waves[index].SpawnWave(_target, this);
        }

        public void Update()
        {
            if (_timer < _timeBetweenUpdates)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                Debug.Log("Updating Waves");
                _timer = 0;
                foreach (Wave wave in _waves)
                {
                    if (wave.active)
                    {
                        wave.UpdateWave();
                    }
                }
            }
        }

        public void SetTrigger(bool triggered)
        {
            _triggered = triggered;
            foreach (Wave wave in _waves)
            {
                wave.Reset();
            }
        }
    }
}
