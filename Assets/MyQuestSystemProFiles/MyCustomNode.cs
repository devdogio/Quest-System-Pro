using System;
using Devdog.General;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;
using Devdog.QuestSystemPro.Dialogue.UI;

namespace Assets.MyQuestSystemProFiles
{
    [Category("Custom/MyCustomNode")]
    public class MyCustomNode : NodeBase
    {
        public override NodeUIBase uiPrefab
        {
            get { return QuestManager.instance.settingsDatabase.myCustomNodeUI; }
        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            UnityEngine.Debug.Log("On MyCustomNode Execute");
            Finish(false);
        }

        public override NodeBase GetNextNode()
        {
            return base.GetNextNode();
        }
    }
}
