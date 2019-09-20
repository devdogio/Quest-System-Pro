using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Default general purpose node, which can be used for regular conversation flow.")]
    [Category("Devdog/General")]
    public class Node : NodeBase
    {

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            Finish(false);
        }
    }
}