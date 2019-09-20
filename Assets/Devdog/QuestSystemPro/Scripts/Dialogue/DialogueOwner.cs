using Devdog.General;
using Devdog.General.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    public class DialogueOwner : MonoBehaviour, IDialogueOwner, ITriggerWindowContainer, ITriggerCallbacks
    {
        [SerializeField]
        [ForceCustomObjectPicker]
        [Required]
        private Dialogue _dialogue;
        public Dialogue dialogue
        {
            get { return _dialogue; }
            set { _dialogue = value; }
        }

        [Header("Owner details")]
        [SerializeField]
        private string _ownerName;
        public string ownerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }

        [SerializeField]
        private Sprite _ownerIcon;
        public Sprite ownerIcon
        {
            get { return _ownerIcon; }
            set { _ownerIcon = value; }
        }

        [SerializeField]
        private DialogueCamera _dialogueCamera;
        public DialogueCamera dialogueCamera
        {
            get { return _dialogueCamera; }
            set { _dialogueCamera = value; }
        }


        public bool playAudio = true;
        public bool playAnimations = true;


        public UIWindow window
        {
            get { return DialogueManager.instance.dialogueUI.window; }
        }


        protected AudioSource audioSource;
        protected Animator animator;

        protected virtual void Awake()
        {
            GetComponents();
        }

        protected virtual void Start()
        {
            dialogue.OnStatusChanged += DialogueOnStatusChanged;
            dialogue.OnCurrentNodeChanged += DialogueOnCurrentNodeChanged;
        }

        protected virtual void OnDestroy()
        {
            dialogue.OnStatusChanged -= DialogueOnStatusChanged;
            dialogue.OnCurrentNodeChanged -= DialogueOnCurrentNodeChanged;
        }

        protected virtual void GetComponents()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
        }

        protected virtual void DialogueOnStatusChanged(DialogueStatus before, DialogueStatus after, Dialogue self, IDialogueOwner owner)
        {
            if (after == DialogueStatus.InActive)
            {
                var trigger = GetComponent<TriggerBase>();
                if (trigger != null)
                {
                    trigger.UnUse();
                }
            }
        }

        protected virtual void DialogueOnCurrentNodeChanged(NodeBase before, NodeBase after)
        {
            if (after.ownerType == DialogueOwnerType.DialogueOwner)
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

        public virtual bool OnTriggerUsed(Player player)
        {
            if (dialogue != null)
            {
                dialogue.StartDialogue(this);
            }

            DevdogLogger.LogVerbose("DialogueOwner consumed trigger callback Use()", this);
            return true; // Dialogue owner consumes the event to avoid others also receiving the trigger used events.
        }

        public virtual bool OnTriggerUnUsed(Player player)
        {
            if (dialogue != null)
            {
                dialogue.Stop();
            }

            DevdogLogger.LogVerbose("DialogueOwner consumed trigger callback UnUse()", this);
            return true; // Dialogue owner consumes the event to avoid others also receiving the trigger used events.
        }
    }
}