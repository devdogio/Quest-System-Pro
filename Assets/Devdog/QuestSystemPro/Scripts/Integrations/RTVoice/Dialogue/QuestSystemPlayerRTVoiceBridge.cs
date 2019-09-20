#if RT_VOICE

using Crosstales.RTVoice;
using Devdog.QuestSystemPro.Dialogue;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integrations.RTVoice
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Integrations/RTVoice/Quest System Player RTVoice Bridge")]
    public class QuestSystemPlayerRTVoiceBridge : MonoBehaviour
    {
        public string voiceCulture = "en";

        public Voice voice { get; set; }

        private static int _activeSpeakerCount = 0;
        private static bool speakerIsActive
        {
            get { return _activeSpeakerCount == 0; }
        }

        protected virtual void Awake()
        { }

        protected virtual void Start()
        {
            DialogueManager.instance.OnCurrentDialogueNodeChanged += DialogueOnCurrentNodeChanged;

            voice = Speaker.VoiceForCulture(voiceCulture);
            Speaker.OnSpeakNativeStart += SpeakerStart;
            Speaker.OnSpeakNativeComplete += SpeakerCompleted;
        }

        private void SpeakerStart(object sender, SpeakNativeEventArgs e)
        {
            _activeSpeakerCount++;
        }

        private void SpeakerCompleted(object sender, SpeakNativeEventArgs e)
        {
            _activeSpeakerCount--;
        }
        
        protected void DialogueOnCurrentNodeChanged(NodeBase before, NodeBase after)
        {
            if (before.ownerType == DialogueOwnerType.Player && speakerIsActive)
            {
                // Stop previous node's talking.
                Speaker.Silence();
            }

            if (after.ownerType == DialogueOwnerType.Player)
            {
                if (after.audioInfo != null && after.audioInfo.audioClip != null)
                {
                    AudioSource source = null;
                    if (DialogueManager.instance.currentDialogueOwner != null)
                    {
                        source = DialogueManager.instance.currentDialogueOwner.transform.GetComponent<AudioSource>();
                    }

                    if (string.IsNullOrEmpty(after.message) == false)
                    {
                        Speaker.Speak(after.message, source, voice, true, 1f, after.audioInfo.volume);
                    }
                }
            }
        }
    }
}

#endif