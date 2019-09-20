#if PLAYMAKER

using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using Devdog.QuestSystemPro.Dialogue.UI;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [System.Serializable]
    [Summary("Call a playmaker event.")]
    [Category("PlayMaker")]
    public class InvokePlayMakerEventNode : NodeBase
    {
        public override NodeUIBase uiPrefab
        {
            get { return null; }
        }

        [ShowInNode]
        public string eventName;

        [NonSerialized]
        public new string message;

        protected InvokePlayMakerEventNode()
            : base()
        {

        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            if (DialogueManager.instance.currentDialogueOwner != null)
            {
                var fsm = DialogueManager.instance.currentDialogueOwner.transform.GetComponent<PlayMakerFSM>();
                if (fsm != null)
                {
                    fsm.SendEvent(eventName);
                }
                else
                {
                    DevdogLogger.LogWarning("No FSM found on dialogue owner. Event not invoked.");
                }
            }
            else
            {
                DevdogLogger.LogWarning("Dialogue not opened through a dialogue owner. No object is set and no FSM can therefore be found.");
            }

            Finish(true);
        }
    }
}

#endif