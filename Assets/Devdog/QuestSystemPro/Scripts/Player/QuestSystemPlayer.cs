using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    /// <summary>
    /// Partial class for the Devdog.General.Player
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class QuestSystemPlayer : MonoBehaviour
    {

        public DialogueCamera dialogueCamera;



        protected AudioSource audioSource;
        protected Animator animator;

        public bool playAudio = true;
        public bool playAnimations = true;

        protected virtual void Awake()
        {
            GetComponents();
        }

        protected virtual void Start()
        {
            DialogueManager.instance.OnCurrentDialogueStatusChanged += DialogueOnStatusChanged;
            DialogueManager.instance.OnCurrentDialogueNodeChanged += DialogueOnCurrentNodeChanged;
        }

        protected virtual void OnDestroy()
        {
            if(DialogueManager.instance != null)
            {
                DialogueManager.instance.OnCurrentDialogueStatusChanged -= DialogueOnStatusChanged;
                DialogueManager.instance.OnCurrentDialogueNodeChanged -= DialogueOnCurrentNodeChanged;
            }
        }

        protected virtual void GetComponents()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
        }

        protected virtual void DialogueOnStatusChanged(DialogueStatus before, DialogueStatus after, Devdog.QuestSystemPro.Dialogue.Dialogue self, IDialogueOwner owner)
        {
            
        }

        protected virtual void DialogueOnCurrentNodeChanged(NodeBase before, NodeBase after)
        {
            if (after.ownerType == DialogueOwnerType.Player)
            {
                if (playAudio && audioSource != null && after.audioInfo.audioClip != null)
                {
                    audioSource.Play(after.audioInfo);
                }

                if (playAnimations && animator != null && after.motionInfo.motion != null)
                {
                    animator.Play(after.motionInfo);
                }
            }
        }
    }
}