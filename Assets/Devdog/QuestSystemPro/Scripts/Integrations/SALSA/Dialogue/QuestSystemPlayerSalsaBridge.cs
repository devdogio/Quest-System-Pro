#if SALSA

using CrazyMinnow.SALSA;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.SALSA
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Integrations/Salsa/Quest System Player Salsa Bridge")]
    public class QuestSystemPlayerSalsaBridge : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private Salsa3D _salsa;

        protected virtual void Awake()
        { }

        protected virtual void Start()
        {
            DialogueManager.instance.OnCurrentDialogueNodeChanged += DialogueOnCurrentNodeChanged;
        }

        protected void DialogueOnCurrentNodeChanged(NodeBase before, NodeBase after)
        {
            if (before.ownerType == DialogueOwnerType.Player && _salsa.isTalking)
            {
                // Stop previous node's talking.
                _salsa.Stop();
            }

            if (after.ownerType == DialogueOwnerType.Player)
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