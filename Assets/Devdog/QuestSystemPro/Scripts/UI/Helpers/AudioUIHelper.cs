using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro.UI
{
    public class AudioUIHelper : MonoBehaviour
    {
        

        public void PlayAudio(AudioClip clip)
        {
            AudioManager.AudioPlayOneShot(new AudioClipInfo()
            {
                audioClip = clip
            });
        }
    }
}