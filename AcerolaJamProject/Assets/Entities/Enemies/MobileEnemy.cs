using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public abstract class MobileEnemy : BaseEnemy
    {
        [Header("Grounded")]
        [SerializeField] private Transform _raycastPoint;
        [SerializeField] private float _raycastDistance = 0.1f;
        [SerializeField] private LayerMask _groundLayers;

        public bool IsGrounded()
        {
            return Physics.Raycast(_raycastPoint.position, Vector3.down, _raycastDistance, _groundLayers);
        }
    }
}