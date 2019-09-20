using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("The split node can be used to run multiple outputs. All children (outgoing edges) will be executed.")]
    [Category("Devdog/Flow control")]
    public class SplitNode : ActionNodeBase
    {

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            Finish(true);
        }
    }
}