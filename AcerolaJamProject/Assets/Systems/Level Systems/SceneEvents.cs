using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Level
{
    public class SceneEvents : MonoBehaviour
    {
        public UnityEvent onAwake;
        public UnityEvent onStart;
        public bool callUpdate;
        public UnityEvent onUpdate;

        private void Awake()
        {
            onAwake.Invoke();
        }

        private void Start()
        {
            onStart.Invoke();
        }

        private void Update()
        {
            if (callUpdate)
                onUpdate.Invoke();
        }
    }
}
