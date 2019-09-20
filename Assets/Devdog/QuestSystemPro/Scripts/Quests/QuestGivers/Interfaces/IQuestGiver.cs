using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public interface IQuestGiver
    {
        Transform transform { get; }
        Quest[] quests { get; set; }

//        Quest[] GetAvailableQuests(ILocalIdentifier identifier);

        void Use();
        void UnUse();

    }
}