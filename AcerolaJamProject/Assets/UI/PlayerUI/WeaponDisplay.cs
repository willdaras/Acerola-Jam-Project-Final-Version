using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Weapons;
using Common;

namespace UI
{
    public class WeaponDisplay : MonoSingleton<WeaponDisplay>
    {
        [SerializeField] private WeaponData _weapon;
        public WeaponData weapon => _weapon;

        [SerializeField] private RawImage _weaponIcon;
        [SerializeField] private TMP_Text _remaining;

        public void SetWeapon(WeaponData weapon)
        {
            if (weapon == null)
            {
                _weaponIcon = null;
                _remaining.text = "";
                return;
            }

            _weapon = weapon;
            _weaponIcon.texture = _weapon.weaponIcon;

            _remaining.text = weapon.ammo.ToString();
        }

        public void SetAmmo(int newAmmo)
        {
            _remaining.text = newAmmo.ToString();
        }
    }
}