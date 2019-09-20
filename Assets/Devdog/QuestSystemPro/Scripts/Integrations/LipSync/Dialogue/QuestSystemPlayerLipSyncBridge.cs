#if LIPSYNC

using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.LipSync
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Integrations/LipSync/Quest System Player LipSync Bridge")]
    public class QuestSystemPlayerLipSyncBridge : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private RogoDigital.Lipsync.LipSync _lipSync;

        protected virtual void Awake()
        { }

        protected virtual void Start()
        {
            DialogueManager.instance.OnCurrentDialogueNodeChanged += DialogueOnCurrentNodeChanged;
        }

        protected void DialogueOnCurrentNodeChanged(NodeBase before, NodeBase after)
        {
            if (before.ownerType == DialogueOwnerType.Player && _lipSync.isPlaying)
            {
                // Stop previous node's talking.
                _lipSync.Stop(true);
            }

            if (after.ownerType == DialogueOwnerType.Player)
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