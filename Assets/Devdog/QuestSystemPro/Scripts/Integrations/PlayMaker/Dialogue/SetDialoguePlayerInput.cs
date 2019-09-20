#if PLAYMAKER

using Devdog.QuestSystemPro.Dialogue;
using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Set a dialogue's user input.")]
    public class SetDialoguePlayerInput : FsmStateAction
    {
        [RequiredField]
        public Dialogue.Dialogue dialogue;

        public FsmString userInput;

        public override void OnEnter()
        {
            var node = dialogue.currentNode as IPlayerInputNode;
            if (node != null)
            {
                node.SetPlayerInputStringAndMoveToNextNode(userInput.Value);
            }
            else
            {
                LogWarning("Current node is not an IPlayerInputNode");
            }

            Finish();
        }
    }
}

#endif