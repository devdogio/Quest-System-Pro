using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("The go to node allows you to go to another node directly, regardless if they're connected or not.")]
    [Category("Devdog/Flow control")]
    public class GoToNode : ActionNodeBase
    {
        [ShowInNode]
        public int goToNodeIndex;

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            Finish(owner.nodes[goToNodeIndex]);
        }

        public override ValidationInfo Validate()
        {
            if (goToNodeIndex >= owner.nodes.Length || goToNodeIndex < 0)
            {
                return new ValidationInfo(ValidationType.Error, "Node with ID " + goToNodeIndex + " doesn't exist");
            }

            if (goToNodeIndex == index)
            {
                return new ValidationInfo(ValidationType.Error, "Can't go to self (go to index is this index)");
            }

            return base.Validate();
        }
    }
}