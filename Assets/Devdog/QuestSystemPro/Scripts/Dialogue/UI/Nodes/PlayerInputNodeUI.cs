using UnityEngine.Events;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.Dialogue.UI
{
    public class PlayerInputNodeUI : DefaultNodeUI
    {
        public class InputEvent : UnityEvent
        {
            
        }


        public InputField userInput;

        public InputEvent inputCorrect = new InputEvent();
        public InputEvent inputInCorrect = new InputEvent();

        public override void Repaint(NodeBase node)
        {
            base.Repaint(node);

            var c = (IPlayerInputNode)currentNode;
            foreach (var decision in decisions)
            {
                decision.button.onClick.RemoveAllListeners();
                decision.button.onClick.AddListener(() =>
                {
                    ValidateUserInput(userInput.text);
                    c.SetPlayerInputStringAndMoveToNextNode(userInput.text);
                });
            }
        }

        protected virtual void ValidateUserInput(string text)
        {
            var c = (IPlayerInputNode) currentNode;
            if (c.IsInputCorrect(text))
            {
                inputCorrect.Invoke();
            }
            else
            {
                inputInCorrect.Invoke();
            }
        }
    }
}