namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public class QuestsContainerSerializationModel
    {
        public QuestSerializationModel[] activeQuests = new QuestSerializationModel[0];
        public QuestSerializationModel[] completedQuests = new QuestSerializationModel[0];
        public QuestSerializationModel[] achievements = new QuestSerializationModel[0];
    }
}
