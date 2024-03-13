using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneLoader : MonoBehaviour
    {
        [Common.SceneReference] public string scene;

        public int sceneIndex => SceneManager.GetSceneByName(scene).buildIndex;

        public void LoadSceneAdditive(bool async)
        {
            if (async)
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            }
        }
        public void LoadSceneSingle(bool async)
        {
            if (async)
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }
    }
}
