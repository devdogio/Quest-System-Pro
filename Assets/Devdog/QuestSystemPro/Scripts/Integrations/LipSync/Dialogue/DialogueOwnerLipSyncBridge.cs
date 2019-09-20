#if LIPSYNC

using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Integration.LipSync
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Integrations/LipSync/Dialogue Owner LipSync Bridge")]
    public class DialogueOwnerLipSyncBridge : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private RogoDigital.Lipsync.LipSync _lipSync;

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
            if (before.ownerType == DialogueOwnerType.DialogueOwner && _lipSync.isPlaying)
            {
                // Stop previous node's talking.
                _lipSync.Stop(true);
            }

            if (after.ownerType == DialogueOwnerType.DialogueOwner)
            {
                if (after.audioInfo != null && after.audioInfo.audioClip != null)
                {
                    LipSyncPlayAudioClip(after);
                }
            }
        }

        public virtual void LipSyncPlayAudioClip(NodeBase node)
        {
            if (node.lipsyncData != null)
            {
                _lipSync.Play(node.lipsyncData);
            }
        }
    }
}

#endif