using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphics
{
    public class LocalTrail : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;
        [SerializeField] private Transform _target;

        private Vector3 lastFramePosition;

        private void Start()
        {
            _trailRenderer = GetComponent<TrailRenderer>();

            lastFramePosition = _target.position;
        }

        private void LateUpdate()
        {
            Vector3 delta = _target.position - lastFramePosition;
            lastFramePosition = _target.position;

            Vector3[] positions = new Vector3[_trailRenderer.positionCount];
            _trailRenderer.GetPositions(positions);

            for (var i = 0; i < _trailRenderer.positionCount; i++)
            {
                positions[i] += delta;
            }

            _trailRenderer.SetPositions(positions);
        }
    }
}