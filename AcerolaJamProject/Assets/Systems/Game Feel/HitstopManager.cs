using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace GameFeel
{
    public class HitstopManager : MonoSingleton<HitstopManager>
    {
        private bool _inHitstop;
        public bool inHitstop => _inHitstop;

        public void HitstopFor(float time)
        {
            if (inHitstop)
                return;
            StartCoroutine(DoHitstop(time));
        }

        private IEnumerator DoHitstop(float time)
        {
            _inHitstop = true;
            Time.timeScale = 0.01f;
            yield return new WaitForSecondsRealtime(time);
            Time.timeScale = 1;
            _inHitstop = false;
        }
    }
}