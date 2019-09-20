using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [HideInCreationWindow]
    public class EntryNode : ActionNodeBase
    {

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            Finish(true);
        }
    }
}