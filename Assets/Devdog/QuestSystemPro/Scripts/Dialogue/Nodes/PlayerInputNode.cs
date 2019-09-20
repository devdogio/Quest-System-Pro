using Devdog.General;
using Devdog.QuestSystemPro.Dialogue.UI;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("The user input node can be used to let the user enter some text and verify it.")]
    [Category("Devdog/Flow control")]
    public class PlayerInputNode : NodeBase, IPlayerInputNode
    {
        [ShowInNode]
        public string expectedString = "";
        public bool ignoreCaps = false;
        public bool ignoreWhiteSpace = true;

        public VariableRef<string> userInput = new VariableRef<string>();
//        public Variable<string> userInput = new Variable<string>();

        public override NodeUIBase uiPrefab
        {
            get { return QuestManager.instance.settingsDatabase.playerInputNodeUIPrefab; }
        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            // No finish, we only finish when the player sets the input string.
        }

        public void SetPlayerInputStringAndMoveToNextNode(string enteredString)
        {
            userInput.value = enteredString;
            Finish(true);
        }

        public override NodeBase GetNextNode()
        {
            if (IsInputCorrect(userInput.value) && edges[0].CanUse(owner))
            {
                return owner.nodes[edges[0].toNodeIndex];
            }

            if (edges[1].CanUse(owner))
            {
                return owner.nodes[edges[1].toNodeIndex];
            }

            return null;
        }

        public virtual bool IsInputCorrect(string enteredString)
        {
            if (ignoreWhiteSpace)
            {
                enteredString = enteredString.Trim();
            }

            if (expectedString == enteredString)
                return true;

            if (ignoreCaps && expectedString.ToLower() == enteredString.ToLower())
                return true;

            return false;
        }

        public override ValidationInfo Validate()
        {
            if (edges.Length == 2)
            {
                return new ValidationInfo(ValidationType.Valid);
            }

            return new ValidationInfo(ValidationType.Error, "The user input node requires 2 outgoing edges (correct and incorrect).");
        }
    }
}