using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class BulletTrail : MonoBehaviour
    {
        public float _destroyTime = 0.1f;

        private void Start()
        {
            Invoke("DestroyTrail", _destroyTime);
        }

        private void DestroyTrail()
        {
            Destroy(gameObject);
        }
    }
}