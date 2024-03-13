using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons
{
    [CreateAssetMenu(fileName ="Weapon Data", menuName ="Weapons/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public Texture2D weaponIcon;
        public int ammo = 1;
        public float drainRatePerSecond = 10;
        public bool drainPerSecond = false;

        public int healthToFill;
        public int ammoToDrain;

        public float screenshakeMagnitude = 1;
        public float screenshakeFrequency = 1.5f;
        public float screenshakeTime = 0.1f;
    }
}