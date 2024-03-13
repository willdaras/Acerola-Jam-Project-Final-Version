using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Combat;
using Weapons;
using Common;

namespace Player.Weapons
{
    public class PlayerWeapon : MonoBehaviour
    {
        private IHealth _health;

        public BaseAttachment weapon => _weapons[_currentIndex];
        [SerializeField] private int _healOnEquipAmount = 50;

        private BaseAttachment _collidingWith;

        public Player player => MonoSingleton<Player>.instance;

        [SerializeField] private Transform _cam;
        [SerializeField] private Transform _weaponParent;
        [SerializeField] private LayerMask _hitLayers;

        private int _remainingAmmo { get { return _weaponAmmos[_currentIndex]; } set { _weaponAmmos[_currentIndex] = value; } }
        private bool _shooting;
        private float _drainTimer;

        [Space]
        [SerializeField] private List<BaseAttachment> _weapons = new List<BaseAttachment> { null, null, null, null };
        private Dictionary<string, int> _weaponIndexes = new Dictionary<string, int> { { "Chaingun", 0 }, { "Sniper", 1 }, { "SuperShotgun", 2 }, { "RocketLauncher", 3 } };
        [SerializeField] private int _currentIndex = 0;
        [SerializeField] private List<int> _weaponAmmos = new List<int>();

        private void Start()
        {
            _health = GetComponent<IHealth>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BaseAttachment attachment))
            {
                if (!attachment.canEquip)
                    return;
                //_collidingWith = attachment;
                EquipWeapon(attachment);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other == null || _collidingWith == null)
                return;

            if (other.gameObject == _collidingWith.gameObject)
            {
                _collidingWith = null;
            }
        }

        public void Attack()
        {
            if (_remainingAmmo <= 0)
                return;
            weapon.Attack();
            if (!weapon.data.drainPerSecond)
                _remainingAmmo--;
            StartCoroutine(Actionable(weapon.actionableTime));
        }

        private IEnumerator Actionable(float actionableTime)
        {
            player.actionable = false;
            yield return new WaitForSeconds(actionableTime);
            player.actionable = true;
        }

        public void OnHeal(InputAction.CallbackContext context)
        {
            if (context.started && _remainingAmmo > weapon.data.ammoToDrain && _health.Health < 100)
            {
                _remainingAmmo -= weapon.data.ammoToDrain;
                _health.Damage(-weapon.data.healthToFill);
            }
        }
        public void OnDrain(InputAction.CallbackContext context)
        {
            if (context.started && _health.Health > weapon.data.healthToFill)
            {
                _remainingAmmo += weapon.data.ammoToDrain;
                _health.Damage(weapon.data.healthToFill);
            }
        }

        public void EquipWeapon(BaseAttachment newWeapon)
        {
            //if (weapon != null)
            //    Destroy(weapon.gameObject);
            Debug.Log(newWeapon.gameObject.name);
            if (_weapons[_weaponIndexes[newWeapon.name]] != null)
            {
                _weaponAmmos[_weaponIndexes[newWeapon.name]] += newWeapon.data.ammo;
                Destroy(newWeapon.gameObject);
                return;
            }

            if (weapon != null)
                weapon.gameObject.SetActive(false);

            _currentIndex = _weaponIndexes[newWeapon.name];
            _weapons[_currentIndex] = newWeapon;
            weapon.transform.parent = _weaponParent;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.transform.localScale = new Vector3(1, 1, 1);
            weapon.canEquip = false;
            if (weapon.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }

            MonoSingleton<UI.WeaponDisplay>.instance.SetWeapon(weapon.data);
            _remainingAmmo = weapon.data.ammo;

        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.started && player.actionable && weapon != null)
            {
                Attack();
                MonoSingleton<GameFeel.ScreenshakeManager>.instance.ScreenshakeFor(weapon.data.screenshakeMagnitude, weapon.data.screenshakeFrequency, weapon.data.screenshakeTime);
            }
            _shooting = context.ReadValue<float>() >= 0.5f;
            if (context.canceled && weapon != null)
            {
                weapon.EndAttack();
            }
        }

        private void Update()
        {
            if (weapon != null)
            {
                weapon.Attacking = _remainingAmmo > 0 && _shooting;

                Vector3 dir;
                if (Physics.Raycast(_cam.transform.position, _cam.forward, out RaycastHit hit, 100, _hitLayers))
                {
                    dir = hit.point - weapon.transform.position;
                }
                else
                {
                    dir = weapon.transform.forward;
                }
                weapon.transform.rotation = Quaternion.LookRotation(dir);


                if (weapon.data.drainPerSecond && weapon.Attacking)
                {
                    _drainTimer += Time.deltaTime;
                    if (_drainTimer >= (1 / weapon.data.drainRatePerSecond))
                    {
                        _remainingAmmo--;
                        _drainTimer = 0;
                        MonoSingleton<GameFeel.ScreenshakeManager>.instance.ScreenshakeFor(weapon.data.screenshakeMagnitude, weapon.data.screenshakeFrequency, weapon.data.screenshakeTime);
                    }
                }
            }

            MonoSingleton<UI.WeaponDisplay>.instance.SetAmmo(_remainingAmmo);
        }

        public void SwitchWeapon1(InputAction.CallbackContext context)
        {
            if (_weapons[0] == null)
                return;
            if (context.started)
            {
                weapon.gameObject.SetActive(false);
                _currentIndex = 0;
                weapon.gameObject.SetActive(true);
                MonoSingleton<UI.WeaponDisplay>.instance.SetWeapon(weapon.data);
            }
        }
        public void SwitchWeapon2(InputAction.CallbackContext context)
        {
            if (_weapons[1] == null)
                return;
            if (context.started)
            {
                weapon.gameObject.SetActive(false);
                _currentIndex = 1;
                weapon.gameObject.SetActive(true);
                MonoSingleton<UI.WeaponDisplay>.instance.SetWeapon(weapon.data);
            }
        }
        public void SwitchWeapon3(InputAction.CallbackContext context)
        {
            if (_weapons[2] == null)
                return;
            if (context.started)
            {
                weapon.gameObject.SetActive(false);
                _currentIndex = 2;
                weapon.gameObject.SetActive(true);
                MonoSingleton<UI.WeaponDisplay>.instance.SetWeapon(weapon.data);
            }
        }
        public void SwitchWeapon4(InputAction.CallbackContext context)
        {
            if (_weapons[3] == null)
                return;
            if (context.started)
            {
                weapon.gameObject.SetActive(false);
                _currentIndex = 3;
                weapon.gameObject.SetActive(true);
                MonoSingleton<UI.WeaponDisplay>.instance.SetWeapon(weapon.data);
            }
        }
    }
}
