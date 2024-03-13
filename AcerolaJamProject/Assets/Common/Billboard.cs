using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class Billboard : MonoBehaviour
    {
        private Transform _cam;

        private void Start()
        {
            _cam = Camera.main.transform;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(-_cam.forward);
        }
    }
}