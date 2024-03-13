using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Common;

namespace Player
{
    public class PlayerShotgun : MonoBehaviour
    {
        public Player player => MonoSingleton<Player>.instance;

        [SerializeField] private float _shotCooldown = 0.5f;
        private float _shotTimer;
        public bool canShoot => _shotTimer >= _shotCooldown;
        [SerializeField] private float _timeBetweenClickAndShoot = 0.2f;

        [SerializeField] private float _actionableTime = 0.4f;

        [SerializeField] private int _pelletCount = 15;
        [SerializeField] private GameObject _pellet;
        [SerializeField] private float _spread = 1;
        [SerializeField] private Transform _shootPos;
        [SerializeField] private Transform _cam;
        [SerializeField] private LayerMask _hitLayers;

        [Space]
        [Header("Feel")]
        [SerializeField] private float _screenShakeTime = 0.1f;
        [SerializeField] private float _screenShakeMagnitude = 1.5f;
        [SerializeField] private float _screenShakeFrequency = 1;

        [Space]
        [SerializeField] private int _maxAmmo = 5;
        public int ammoCount;
        [SerializeField] private float _regenPerAmmo = 0.5f;
        private float _ammoRegenTimer;

        public UnityEvent onShoot;

        private void Start()
        {

        }

        private void Update()
        {
            if (_shotTimer < _shotCooldown)
            {
                _shotTimer += Time.deltaTime;
            }
            if (_ammoRegenTimer < _regenPerAmmo && ammoCount < _maxAmmo)
            {
                _ammoRegenTimer += Time.deltaTime;
                if (_ammoRegenTimer >= _regenPerAmmo)
                {
                    ammoCount++;
                    _ammoRegenTimer = 0;
                }
            }
        }

        private void Shoot()
        {
            ammoCount--;

            Vector3 targetPos;

            if (Physics.Raycast(_cam.position, _cam.forward, out RaycastHit hit, 100, _hitLayers))
            {
                targetPos = hit.point;
            }
            else
            {
                targetPos = _cam.transform.position + _cam.transform.forward * 50;
            }

            for (int i = 0; i < _pelletCount; i++)
            {
                Vector3 direction = (targetPos - _shootPos.position).normalized;
                Vector3 spread = _cam.transform.up * Random.Range(-_spread, _spread);
                spread += _cam.transform.right * Random.Range(-_spread, _spread);

                direction += spread.normalized * Random.Range(0f, 0.2f);

                Instantiate(_pellet, _shootPos.position, Quaternion.LookRotation(direction));
            }

            MonoSingleton<GameFeel.ScreenshakeManager>.instance.ScreenshakeFor(_screenShakeMagnitude, _screenShakeFrequency, _screenShakeTime);
        }

        private IEnumerator Actionable(float actionableTime)
        {
            player.actionable = false;
            yield return new WaitForSeconds(actionableTime);
            player.actionable = true;
        }
        private IEnumerator ShootShotgun()
        {
            _shotTimer = 0;
            StartCoroutine(Actionable(_actionableTime));
            onShoot.Invoke();
            yield return new WaitForSeconds(_timeBetweenClickAndShoot);
            Shoot();
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.started && player.actionable && canShoot && ammoCount > 0)
            {
                StartCoroutine(ShootShotgun());
            }
        }
    }
}