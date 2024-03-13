namespace UI
{
    [System.Serializable]
    public struct Prefs
    {
        public int resolutionIndex;
        public int graphicsIndex;
        public int fullscreen;
        public int crt;
        public int pixelated;
        public int blur;

        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;

        public float mouseSensitivity;
    }
}
