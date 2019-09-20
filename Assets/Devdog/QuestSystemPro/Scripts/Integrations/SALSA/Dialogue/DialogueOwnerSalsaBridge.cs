#if SALSA

using CrazyMinnow.SALSA;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Integration.SALSA
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Integrations/Salsa/Dialogue Owner Salsa Bridge")]
    public class DialogueOwnerSalsaBridge : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private Salsa3D _salsa;

        private IDialogueOwner _owner;
        protected virtual void Awake()
        {
            _owner = GetComponent<IDialogueOwner>();
            Assert.IsNotNull(_owner, "No IDialogueOwner found!");
        }

        protected virtual void Start()
        {
            _owner.dialogue.OnCurrentNodeChanged += DialogueOnCurrentNodeChanged;
        }

        protected void DialogueOnCurrentNodeChanged(NodeBase before, NodeBase after)
        {
            if (before.ownerType == DialogueOwnerType.DialogueOwner && _salsa.isTalking)
            {
                // Stop previous node's talking.
                _salsa.Stop();
            }

            if (after.ownerType == DialogueOwnerType.DialogueOwner)
            {
                if (after.audioInfo != null && after.audioInfo.audioClip != null)
                {
                    SalsaPlayAudioClip(after.audioInfo);
                }
            }
        }

        public virtual void SalsaPlayAudioClip(AudioClipInfo audioClip)
        {
            _salsa.SetAudioClip(audioClip.audioClip);
            _salsa.Play();
        }

        public virtual void SalsaPlayAudioClip(LocalizedAudioClipInfo audioClip)
        {
            _salsa.SetAudioClip(audioClip.audioClip.val);
            _salsa.Play();
        }
    }
}

#endif