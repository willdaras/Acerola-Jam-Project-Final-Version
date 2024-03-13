using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Common;

namespace GameFeel
{
    public class ScreenshakeManager : MonoSingleton<ScreenshakeManager>
    {
        [SerializeField] private CinemachineVirtualCamera _vcam;
        private CinemachineBasicMultiChannelPerlin _noise;

        private void Start()
        {
            _noise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void StartScreenshake(float intensity, float frequency)
        {
            _noise.m_AmplitudeGain = intensity;
            _noise.m_FrequencyGain = frequency;
        }
        public void StopScreenshake()
        {
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
        }

        public void ScreenshakeFor(float intensity, float frequency, float time)
        {
            StartCoroutine(Screenshake(intensity, frequency, time));
        }

        private IEnumerator Screenshake(float intensity, float frequency, float time)
        {
            StartScreenshake(intensity, frequency);
            yield return new WaitForSeconds(time);
            StopScreenshake();
        }
    }
}