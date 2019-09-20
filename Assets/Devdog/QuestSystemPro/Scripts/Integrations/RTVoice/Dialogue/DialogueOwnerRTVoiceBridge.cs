#if RT_VOICE

using Crosstales.RTVoice;
using Devdog.QuestSystemPro.Dialogue;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Integrations.RTVoice
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Integrations/RTVoice/Dialogue Owner RTVoice Bridge")]
    public class DialogueOwnerRTVoiceBridge : MonoBehaviour
    {
        public string voiceCulture = "en";

        public Voice voice { get; set; }

        private static int _activeSpeakerCount = 0;
        private static bool speakerIsActive
        {
            get { return _activeSpeakerCount == 0; }
        }

        private IDialogueOwner _owner;
        protected virtual void Awake()
        {
            _owner = GetComponent<IDialogueOwner>();
            Assert.IsNotNull(_owner, "No IDialogueOwner found!");
        }

        protected virtual void Start()
        {
            _owner.dialogue.OnCurrentNodeChanged += DialogueOnCurrentNodeChanged;

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
            if (before.ownerType == DialogueOwnerType.DialogueOwner && speakerIsActive)
            {
                // Stop previous node's talking.
                Speaker.Silence();
            }

            if (after.ownerType == DialogueOwnerType.DialogueOwner)
            {
                if (after.audioInfo != null && after.audioInfo.audioClip != null)
                {
                    if (string.IsNullOrEmpty(after.message) == false)
                    {
                        Speaker.Speak(after.message, _owner.transform.GetComponent<AudioSource>(), voice, true, 1f, after.audioInfo.volume);
                    }
                }
            }
        }
    }
}

#endif