using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [CreateAssetMenu(menuName = QuestSystemPro.ProductName + "/Language Database")]
    public partial class LanguageDatabase : ScriptableObject
    {
        [Category("Quests")]
        public MultiLangString missingCompletedQuest = new MultiLangString("", "You have to finish of range.");
        public MultiLangString canNotAcceptQuestRequiresCompletedQuest = new MultiLangString("", "Can not accept quest.");
        public MultiLangString canNotAcceptQuestReachedMaxRepeatTimes = new MultiLangString("", "Can not accept quest, repeated maximum times.");
        public MultiLangString canNotAcceptQuestMaxActiveQuests = new MultiLangString("", "Can not accept quest, you have to many active quests.");

        [Category("Quest tasks")]
        public MultiLangString canNotCompleteQuestTasksAreNotCompleted = new MultiLangString("", "Can not complete quest, tasks are not completed.");
        public MultiLangString canNotCompleteQuestOverTimeLimit = new MultiLangString("", "Can not complete quest, time limit reached.");
        public MultiLangString questTaskNotCompleted = new MultiLangString("", "Can not complete task, progress is insufficient.");

        [Category("Other")]
        public MultiLangString outOfRange = new MultiLangString("", "You're out of range.");

    }
}