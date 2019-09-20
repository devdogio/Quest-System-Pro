using UnityEngine;
using Devdog.QuestSystemPro.Dialogue;

namespace Devdog.QuestSystemPro.Demo
{
    public sealed class DialogueOwnerUIWorldspacePositioner : MonoBehaviour
    {
        private void Start()
        {
            DialogueManager.instance.OnCurrentDialogueChanged += OnCurrentDialogueChanged;
        }

        private void OnCurrentDialogueChanged(Dialogue.Dialogue before, Dialogue.Dialogue after, IDialogueOwner owner)
        {
            if (owner != null)
            {
                transform.position = owner.transform.position;
            }
        }
    }
}