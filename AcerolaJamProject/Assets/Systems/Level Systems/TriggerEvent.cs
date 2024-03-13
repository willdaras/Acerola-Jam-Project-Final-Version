using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Level
{
    public class TriggerEvent : MonoBehaviour
    {
        public UnityEvent onTriggerEnter;
        public UnityEvent onTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke();
        }
        private void OnTriggerExit(Collider other)
        {
            onTriggerExit.Invoke();
        }
    }
}