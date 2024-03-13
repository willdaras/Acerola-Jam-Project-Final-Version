using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class DisableAfter : MonoBehaviour
    {
        [SerializeField] private float _time = 3;
        [SerializeField] private bool _realTime = false;
        [SerializeField] private bool _destroy = false;

        private void OnEnable()
        {
            StartCoroutine(Disable());
        }

        private IEnumerator Disable()
        {
            yield return _realTime ? new WaitForSecondsRealtime(_time) : new WaitForSeconds(_time);
            if (_destroy)
            {
                Destroy(gameObject);
                yield break;
            }
            gameObject.SetActive(false);
        }
    }
}