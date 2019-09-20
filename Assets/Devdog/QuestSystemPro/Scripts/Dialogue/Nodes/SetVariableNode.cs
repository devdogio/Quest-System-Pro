using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Set a dialogue variable.")]
    public class SetVariableNode<T> : ActionNodeBase
    {
        [ShowInNode]
        public VariableRef<T> variable = new VariableRef<T>();

        [ShowInNode]
        public T value;

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            variable.value = value;
            Finish(true);
        }
    }
}