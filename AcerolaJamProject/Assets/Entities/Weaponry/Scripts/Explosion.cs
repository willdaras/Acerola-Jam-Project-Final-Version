using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private Shader _unlit;

        [SerializeField] private MeshRenderer _mesh;

        private Material _material;

        [SerializeField] private float _activeTime = 0.5f;
        private float _timer;
        [SerializeField] private Gradient _colourOverLifetime;
        [SerializeField] private float _maxSize = 3;

        [SerializeField] private AnimationCurve _colourScaleCurve;

        private float _currentSize => _colourScaleCurve.Evaluate(_timer / _activeTime) * _maxSize;

        private void Start()
        {
            _material = new Material(_unlit);
            _mesh.material = _material;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            _material.SetColor("_BaseColor", _colourOverLifetime.Evaluate(_colourScaleCurve.Evaluate(_timer / _activeTime)));

            transform.localScale = new Vector3(_currentSize, _currentSize, _currentSize);

            if (_timer >= _activeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}