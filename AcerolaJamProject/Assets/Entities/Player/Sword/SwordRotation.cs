using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class SwordRotation : MonoBehaviour
    {
        [SerializeField] private Transform _cam;
        [SerializeField] private Transform _follow;
        [SerializeField] private float _rotateSpeed = 0.1f;

        [SerializeField] private float _moveSpeed = 0.1f;

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = _follow.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, _cam.rotation, 1 / _rotateSpeed * Time.deltaTime);
            //Vector3.Lerp(transform.forward, _cam.forward, _rotateSpeed * Time.deltaTime));
        }
    }
}
