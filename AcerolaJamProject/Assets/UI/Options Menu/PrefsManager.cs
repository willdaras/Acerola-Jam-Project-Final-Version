using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PrefsManager
    {
        public static void SetPrefs(Prefs prefs)
        {
            PlayerPrefs.SetInt("graphics_index", prefs.graphicsIndex);
            PlayerPrefs.SetInt("resolution_index", prefs.resolutionIndex);
            PlayerPrefs.SetInt("fullscreen", prefs.fullscreen);
            PlayerPrefs.SetInt("pixelated", prefs.pixelated);
            PlayerPrefs.SetInt("crt", prefs.crt);
            PlayerPrefs.SetInt("blur", prefs.blur);

            PlayerPrefs.SetFloat("master_volume", prefs.masterVolume);
            PlayerPrefs.SetFloat("music_volume", prefs.musicVolume);
            PlayerPrefs.SetFloat("sfx_volume", prefs.sfxVolume);

            PlayerPrefs.SetFloat("mouse_sensitivity", prefs.mouseSensitivity);
        }

        public static Prefs GetPrefs()
        {
            Prefs prefs = new Prefs();
            prefs.graphicsIndex = PlayerPrefs.GetInt("graphics_index");
            prefs.resolutionIndex = PlayerPrefs.GetInt("resolution_index");
            prefs.fullscreen = PlayerPrefs.GetInt("fullscreen");
            prefs.pixelated = PlayerPrefs.GetInt("pixelated");
            prefs.crt = PlayerPrefs.GetInt("crt");
            prefs.blur = PlayerPrefs.GetInt("blur");

            prefs.masterVolume = PlayerPrefs.GetFloat("master_volume");
            prefs.musicVolume = PlayerPrefs.GetFloat("music_volume");
            prefs.sfxVolume = PlayerPrefs.GetFloat("sfx_volume");

            prefs.mouseSensitivity = PlayerPrefs.GetFloat("mouse_sensitivity");
            return prefs;
        }
    }
}
