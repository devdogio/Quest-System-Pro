using System;
using Devdog.General;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Opens or closes the quest window.")]
    [Category("Devdog/Quests")]
    public class ShowQuestWindowNode : ActionNodeBase
    {
        [ShowInNode]
        [Required]
        public Asset<Quest> quest;

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            Assert.IsNotNull(QuestManager.instance.questWindowUI, "Quest window not set, yet trying to ShowQuestWindow");
            QuestManager.instance.questWindowUI.Repaint(quest.val);
            Finish(true);
        }
    }
}