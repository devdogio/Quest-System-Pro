using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Managers/Dialogue Manager")]
    public partial class DialogueManager : MonoBehaviour
    {
        public delegate void CurrentDialogueChanged(Dialogue before, Dialogue after, IDialogueOwner owner);
        public delegate void CurrentDialogueNodeChanged(NodeBase before, NodeBase after);
        public delegate void CurrentDialogueStatusChanged(DialogueStatus before, DialogueStatus after, Dialogue self, IDialogueOwner owner);

        public event CurrentDialogueChanged OnCurrentDialogueChanged;
        public event CurrentDialogueNodeChanged OnCurrentDialogueNodeChanged;
        public event CurrentDialogueStatusChanged OnCurrentDialogueStatusChanged;
        

        [Header("Scene references")]
        [Required]
        public DialogueUI dialogueUI;


        [NonSerialized]
        private Dialogue _currentDialogue;
        public Dialogue currentDialogue
        {
            get { return _currentDialogue; }
            protected set { _currentDialogue = value; }
        }

        [NonSerialized]
        private IDialogueOwner _currentDialogueOwner;
        public IDialogueOwner currentDialogueOwner
        {
            get { return _currentDialogueOwner; }
            protected set { _currentDialogueOwner = value; }
        }

        private static DialogueManager _instance;
        public static DialogueManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DialogueManager>();
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (GetComponent<DialogueCameraManager>() == null)
            {
                gameObject.AddComponent<DialogueCameraManager>();
            }
        }

        protected virtual void Start()
        {
            

        }

        /// <summary>
        /// Note: This clears the dialogueOwner on this manager. If you want to set the manager call SetCurrentDialogueOwner, which can also set the dialogue.
        /// </summary>
        /// <param name="dialogue"></param>
        public virtual void SetCurrentDialogue(Dialogue dialogue)
        {
            SetCurrentDialogue(dialogue, null);
        }

        public virtual void SetCurrentDialogue(Dialogue dialogue, IDialogueOwner owner)
        {
            if (_currentDialogue != null)
            {
                _currentDialogue.OnCurrentNodeChanged -= NotifyCurrentDialogueNodeChanged;
                _currentDialogue.OnStatusChanged -= NotifyCurrentDialogueStatusChanged;
            }

            var before = _currentDialogue;
            _currentDialogue = dialogue;
            _currentDialogueOwner = owner;

            if (_currentDialogue != null)
            {
                _currentDialogue.OnCurrentNodeChanged += NotifyCurrentDialogueNodeChanged;
                _currentDialogue.OnStatusChanged += NotifyCurrentDialogueStatusChanged;
            }

            if (before != _currentDialogue)
            {
                NotifyCurrentDialogueChanged(before, _currentDialogue, _currentDialogueOwner);
            }
        }

        protected virtual void NotifyCurrentDialogueChanged(Dialogue before, Dialogue after, IDialogueOwner owner)
        {
            if (before != null)
            {
                before.status = DialogueStatus.InActive;
//                before.Stop();
            }

            if (OnCurrentDialogueChanged != null)
            {
                OnCurrentDialogueChanged(before, after, owner);
            }
        }

        protected virtual void NotifyCurrentDialogueStatusChanged(DialogueStatus before, DialogueStatus after, Dialogue self, IDialogueOwner owner)
        {
            if (OnCurrentDialogueStatusChanged != null)
            {
                OnCurrentDialogueStatusChanged(before, after, self, owner);
            }
        }

        protected virtual void NotifyCurrentDialogueNodeChanged(NodeBase before, NodeBase after)
        {
            if (OnCurrentDialogueNodeChanged != null)
            {
                OnCurrentDialogueNodeChanged(before, after);
            }
        }

        public virtual void Reset()
        {
            Dialogue dialogue = currentDialogue;
            if(currentDialogueOwner != null)
            {
                dialogue = dialogue ?? currentDialogueOwner.dialogue;
            }

            if(dialogue != null)
            {
                dialogue.Stop(true);
            }

            currentDialogue = null;
            currentDialogueOwner = null;
        }
    }
}