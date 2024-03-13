using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool Paused;

        public UnityEvent onPause;
        public UnityEvent onUnpause;

        public void Pause()
        {
            Paused = true;
            onPause.Invoke();
        }
        public void Unpause()
        {
            Paused = false;
            Time.timeScale = 1;
            onUnpause.Invoke();
        }

        private void Update()
        {
            if (Paused)
            {
                Time.timeScale = 0;
            }
        }

        public void TogglePause()
        {
            if (Paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                TogglePause();
            }
        }
    }
}