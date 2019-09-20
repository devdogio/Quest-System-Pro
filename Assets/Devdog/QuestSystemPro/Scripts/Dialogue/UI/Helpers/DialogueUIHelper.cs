using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue.UI
{
    public class DialogueUIHelper : MonoBehaviour
    {
        public void NextNode()
        {
            if (DialogueManager.instance.currentDialogue != null)
                DialogueManager.instance.currentDialogue.MoveToNextNode();
        }

        public void StopDialogue()
        {
            if (DialogueManager.instance.currentDialogue != null)
                DialogueManager.instance.currentDialogue.Stop();
        }

    }
}