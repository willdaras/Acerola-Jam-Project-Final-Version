using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using TMPro;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace UI
{
    public class OptionsMenu : MonoBehaviour
    {
        [Header("Graphics")]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        private Resolution[] _resolutions;

        [SerializeField] private TMP_Dropdown _graphicsQualityDropdown;
        [SerializeField] private Material _crtMaterial;
        [SerializeField] private UniversalRendererData _rendererData;
        [SerializeField] private Toggle _fullscreenToggle;
        [SerializeField] private Toggle _crtToggle;
        [SerializeField] private Toggle _pixelationToggle;
        [SerializeField] private Toggle _blurToggle;

        [Space]
        [Header("Audio")]
        [SerializeField] private UnityEngine.Audio.AudioMixer _mixer;

        [SerializeField] private Slider _master;
        [SerializeField] private Slider _music;
        [SerializeField] private Slider _sfx;

        [SerializeField] private InputActionReference lookAction;

        public UnityEvent onPrefChange;

        public Prefs prefs;

        private void OnEnable()
        {
            GetInitialValues();
            SetInitialValues();
            RefreshResolutionDropdown();
        }

        private void GetInitialValues()
        {
            prefs = PrefsManager.GetPrefs();
        }
        private void SetInitialValues()
        {
            _resolutionDropdown.value = prefs.resolutionIndex;
            Screen.SetResolution(_resolutions[prefs.resolutionIndex].width, _resolutions[prefs.resolutionIndex].height, true);
            _resolutionDropdown.RefreshShownValue();

            _graphicsQualityDropdown.value = prefs.graphicsIndex;
            QualitySettings.SetQualityLevel(prefs.graphicsIndex);
            _graphicsQualityDropdown.RefreshShownValue();

            _fullscreenToggle.isOn = prefs.fullscreen == 1;
            Screen.fullScreen = prefs.fullscreen == 1;

            _pixelationToggle.isOn = prefs.pixelated == 1;
            //((FullScreenPassRendererFeature)_rendererData.rendererFeatures.Find(r => r.name == "Blur Effect")).passMaterial.SetInt("_Pixelation", (prefs.pixelated == 1) ? 2 : 1);
            _crtMaterial.SetFloat("_Pixelation", (prefs.pixelated == 1) ? 3 : 1);

            _crtToggle.isOn = prefs.crt == 1;
            _rendererData.rendererFeatures.Find(r => r.name == "CRT Effect").SetActive(prefs.crt == 1);

            _blurToggle.isOn = prefs.blur == 1;
            _rendererData.rendererFeatures.Find(r => r.name == "Blur Effect").SetActive(prefs.blur == 1);

            _master.value = prefs.masterVolume;
            _mixer.SetFloat("MasterVolume", prefs.masterVolume);
            _music.value = prefs.musicVolume;
            _mixer.SetFloat("MusicVolume", prefs.musicVolume);
            _sfx.value = prefs.sfxVolume;
            _mixer.SetFloat("SFXVolume", prefs.sfxVolume);
        }
        public void SavePrefs()
        {
            PrefsManager.SetPrefs(prefs);
        }

        #region Graphics
        private void RefreshResolutionDropdown()
        {
            _resolutions = Screen.resolutions;

            _resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + "x" + _resolutions[i].height;
                options.Add(option);
            }

            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = prefs.resolutionIndex;
            _resolutionDropdown.RefreshShownValue();
        }
        public void SetResolution(int index)
        {
            prefs.resolutionIndex = index;
            _resolutions = Screen.resolutions;
            Debug.Log(_resolutions);
            Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, true);
            onPrefChange.Invoke();
        }

        public void SetGraphicsQuality(int quality)
        {
            prefs.graphicsIndex = quality;
            QualitySettings.SetQualityLevel(quality);
            onPrefChange.Invoke();
        }

        public void SetFullscreen(bool value)
        {
            prefs.fullscreen = value ? 1 : 0;
            Screen.fullScreen = value;
            onPrefChange.Invoke();
        }

        public void SetPixelated(bool value)
        {
            prefs.pixelated = value ? 1 : 0;
            Debug.Log(_crtMaterial.GetInt("_Pixelation"));
            //((FullScreenPassRendererFeature)_rendererData.rendererFeatures.Find(r => r.name == "Blur Effect")).passMaterial.SetInt("_Pixelation", value ? 2 : 1);
            _crtMaterial.SetInt("_Pixelation", value ? 2 : 1);
            onPrefChange.Invoke();
        }

        public void SetCRT(bool value)
        {
            prefs.crt = value ? 1 : 0;
            _rendererData.rendererFeatures.Find(r => r.name == "CRT Effect").SetActive(value);
            onPrefChange.Invoke();
        }

        public void SetBlur(bool value)
        {
            prefs.blur = value ? 1 : 0;
            _rendererData.rendererFeatures.Find(r => r.name == "Blur Effect").SetActive(value);
            onPrefChange.Invoke();
        }
        #endregion

        #region Audio
        public void SetMasterVolume(float volume)
        {
            prefs.masterVolume = volume;
            _mixer.SetFloat("MasterVolume", volume);
            onPrefChange.Invoke();
        }
        public void SetMusicVolume(float volume)
        {
            prefs.musicVolume = volume;
            _mixer.SetFloat("MusicVolume", volume);
            onPrefChange.Invoke();
        }
        public void SetSFXVolume(float volume)
        {
            prefs.sfxVolume = volume;
            _mixer.SetFloat("SFXVolume", volume);
            onPrefChange.Invoke();
        }
        #endregion

        public void SetSensitivity(float sensitivity)
        {
            prefs.mouseSensitivity = sensitivity;
            lookAction.action.ApplyBindingOverride(new InputBinding
            {
                overrideProcessors = $"scaleVector2(x={sensitivity}, y={sensitivity})"
            });
            onPrefChange.Invoke();
        }
    }
}
