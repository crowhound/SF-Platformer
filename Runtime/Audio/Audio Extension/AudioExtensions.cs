using UnityEngine;

namespace SF
{
    public static class AudioExtensions
    {
        public static float VolumeToDecibal(this float volume)
        {
            if (volume > 1)
                volume = 1;

            return Mathf.Log10(volume) * 20;
        }
    }
}
