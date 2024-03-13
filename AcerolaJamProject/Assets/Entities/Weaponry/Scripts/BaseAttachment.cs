using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public abstract class BaseAttachment : MonoBehaviour
    {
        public float actionableTime = 0.3f;
        public bool canEquip;
        public bool isNew = true;

        public WeaponData data;

        public void SetEquipable(bool canEquip) => this.canEquip = canEquip;

        public abstract void Attack();
        public bool Attacking;
        public abstract void EndAttack();
    }
}