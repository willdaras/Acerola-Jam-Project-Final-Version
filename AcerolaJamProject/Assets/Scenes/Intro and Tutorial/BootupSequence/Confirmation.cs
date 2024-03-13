using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Common;

namespace UI
{
    public class Confirmation : MonoBehaviour
    {
        public GameObject confirm;
        public GameObject deny;
        private bool _pressed;
        public float waitTime = 3;

        [SceneReference] public string scene;

        public void OnConfirm()
        {
            if (_pressed)
                return;
            confirm.SetActive(true);

            StartCoroutine(LoadScene());
        }
        public void OnDeny()
        {
            if (_pressed)
                return;
            deny.SetActive(true);

            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(scene);
        }
    }
}