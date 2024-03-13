using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Weapons;

namespace Player
{
    public class Player : MonoSingleton<Player>
    {
        public bool actionable = true;

        public PlayerShotgun shotgun;
        public PlayerSword sword;
    }
}
