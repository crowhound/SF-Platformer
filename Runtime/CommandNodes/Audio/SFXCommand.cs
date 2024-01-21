using UnityEngine;
using SF.CommandModule;

namespace SF
{
    [System.Serializable]
    public class SFXCommand : CommandNode, ICommand
    {
        public AudioClip SFXClip;
        public float Volume;
        public AudioSource SFXSource;

        public async override Awaitable Use() 
        {
            if (SFXClip == null)
                return;
                
            await  PlayAudio();
        }

        private async Awaitable PlayAudio()
        {
            SFXSource.PlayOneShot(SFXClip,Volume);
        }
    }
}
