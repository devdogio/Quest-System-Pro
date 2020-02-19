using System;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.Dialogue.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Settings")]
        public bool hideDialogueOwnerNameOnPlayerNode = true;
        public bool hideDialogueOwnerIconOnPlayerNode = true;

        [Header("Audio & Visuals")]
        public Image dialogueSpeakerImage;
        public Text dialogueOwnerName;

        [Header("Misc")]
        public RectTransform nodeUIContainer;

        public Dialogue currentDialogue
        {
            get { return DialogueManager.instance.currentDialogue; }
        }

        public UIWindow window { get; protected set; }
        protected NodeUIBase currentNodeUI;


        protected virtual void Awake()
        {
            window = GetComponent<UIWindow>();
        }

        protected virtual void Start()
        {
            window.OnHide += WindowOnHide;
            DialogueManager.instance.OnCurrentDialogueStatusChanged += OnCurrentDialogueStatusChanged;
            DialogueManager.instance.OnCurrentDialogueNodeChanged += OnCurrentDialogueNodeChanged;
        }

        protected virtual void OnDestroy()
        {
            window.OnHide -= WindowOnHide;

            if(DialogueManager.instance != null)
            {
                DialogueManager.instance.OnCurrentDialogueStatusChanged -= OnCurrentDialogueStatusChanged;
                DialogueManager.instance.OnCurrentDialogueNodeChanged -= OnCurrentDialogueNodeChanged;
            }
        }

        protected virtual void WindowOnHide()
        {
            if(currentDialogue != null)
            {
                currentDialogue.Stop();
            }
        }

        protected virtual void OnCurrentDialogueNodeChanged(NodeBase before, NodeBase after)
        {
            if (currentNodeUI != null)
            {
                Destroy(currentNodeUI.gameObject);
            }

            if (after.uiPrefab == null)
            {
                DevdogLogger.LogWarning("No prefab found for node. Make sure to assign your UI prefabs to the manager", this);
                return;
            }

            if (after.ownerType != DialogueOwnerType.DialogueOwner)
            {
                SetDialogueSpeakerIcon(hideDialogueOwnerIconOnPlayerNode ? null : QuestManager.instance.settingsDatabase.playerDialogueIcon);
                if(dialogueOwnerName != null)
                {
                    dialogueOwnerName.text = (hideDialogueOwnerNameOnPlayerNode ? string.Empty : QuestManager.instance.settingsDatabase.playerName);
                }
            }
            else
            {
                var owner = DialogueManager.instance.currentDialogueOwner;
                if (owner != null)
                {
                    SetDialogueSpeakerIcon(owner.ownerIcon);
                    if (dialogueOwnerName != null)
                    {
                        dialogueOwnerName.text = owner.ownerName;
                    }
                }
            }

            currentNodeUI = UnityEngine.Object.Instantiate<NodeUIBase>(after.uiPrefab);
            currentNodeUI.transform.SetParent(nodeUIContainer);
            UIUtility.ResetTransform(currentNodeUI.transform);
            UIUtility.InheritParentSize(currentNodeUI.transform);

            currentNodeUI.Repaint(after);
        }

        protected void SetDialogueSpeakerIcon(Sprite sprite)
        {
            if (dialogueSpeakerImage != null)
            {
                dialogueSpeakerImage.gameObject.SetActive(sprite != null);
                dialogueSpeakerImage.sprite = sprite;
            }
        }

        protected virtual void OnCurrentDialogueStatusChanged(DialogueStatus before, DialogueStatus after, Dialogue self, IDialogueOwner owner)
        {
            switch (after)
            {
                case DialogueStatus.InActive:
                    window.Hide();
                    break;
                case DialogueStatus.Active:
                    window.Show();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("after", after, null);
            }
        }
    }
}