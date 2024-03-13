using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

namespace Level
{
    public class CheckPoint : MonoBehaviour
    {
        [SerializeField] private string _playerTag = "Player";
        private GameObject _player;

        [SerializeField] private List<Pair> _spawners;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_playerTag))
            {
                _player = other.GetComponentInParent<Player.Player>().gameObject;
                Common.MonoSingleton<CheckpointManager>.instance.SetCheckpoint(this);
                Debug.Log($"Checkpoint set to {gameObject.name}");
            }
        }

        public void ResetFromCheckpoint()
        {
            _player.transform.position = transform.position;
            _player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _player.GetComponent<Player.PlayerHealth>().Damage(-500);

            foreach (Pair pair in _spawners)
            {
                pair.spawner.SetTrigger(pair.triggered);
            }
        }

        [System.Serializable] private struct Pair { public Targeting.EnemySpawner spawner; public bool triggered; }
    }
}