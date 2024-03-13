using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace Player
{
    public class Zoom : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _vcam;
        [SerializeField] private float _zoomFOV = 30;
        private float _fov;

        public void OnZoom(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _fov = _vcam.m_Lens.FieldOfView;
                _vcam.m_Lens.FieldOfView = _zoomFOV;
            }
            if (context.canceled)
            {
                _vcam.m_Lens.FieldOfView = _fov;
            }
        }
    }
}