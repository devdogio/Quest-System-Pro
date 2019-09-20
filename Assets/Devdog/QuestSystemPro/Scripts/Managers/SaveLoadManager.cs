using System.Collections.Generic;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Managers/Save Load Manager")]
    public partial class SaveLoadManager : MonoBehaviour
    {
        public bool loadOnAwake = true;
        public bool saveOnApplicationQuit = true;

        public const string PlayerPrefsQuestStatesKey = "QUEST_SYSTEM_PRO_QUEST_STATES_";

        protected virtual void Awake()
        {
            if (loadOnAwake)
            {
                LoadAllQuestsAndAchievementsForAll();
            }
        }

        protected virtual void OnApplicationQuit()
        {
            if (saveOnApplicationQuit)
            {
                SaveAllQuestsAndAchievementsForAll();
            }
        }

        public virtual void SaveAllQuestsAndAchievementsForAll()
        {
            var db = QuestManager.instance.GetAllQuestStates();
            foreach (var kvp in db)
            {
                SaveAllQuestsAndAchievementsFor(kvp.Key);
            }

            DevdogLogger.Log("Saved " + db.Count + " player's quests");
        }

        public virtual void SaveAllQuestsAndAchievementsFor(ILocalIdentifier localIdentifier)
        {
            var model = CreateQuestsContainerSerializationModel(QuestManager.instance.GetQuestStates(localIdentifier));
            SaveQuestsContainerModel(PlayerPrefsQuestStatesKey + localIdentifier.ToString(), model);
        }

        protected QuestsContainerSerializationModel CreateQuestsContainerSerializationModel(QuestsContainer container)
        {
            if (container == null)
            {
                return new QuestsContainerSerializationModel();
            }

            var model = new QuestsContainerSerializationModel()
            {
                activeQuests = container.activeQuests.Select(o => new QuestSerializationModel(o)).ToArray(),
                completedQuests = container.completedQuests.Select(o => new QuestSerializationModel(o)).ToArray(),
                achievements = container.achievements.Select(o => new QuestSerializationModel(o)).ToArray()
            };

            return model;
        }

        protected virtual void SaveQuestsContainerModel(string key, QuestsContainerSerializationModel model)
        {
            var json = JsonSerializer.Serialize(model, typeof(QuestsContainerSerializationModel), null);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public virtual void LoadAllQuestsAndAchievementsForAll()
        {
            var db = QuestManager.instance.GetAllQuestStates();
            foreach (var kvp in db)
            {
                LoadAllQuestsAndAchievementsFor(kvp.Key);
            }
        }

        public virtual void LoadAllQuestsAndAchievementsFor(ILocalIdentifier localIdentifier)
        {
            var db = QuestManager.instance.GetQuestStates(localIdentifier);
            var model = LoadQuestsContainerModel(PlayerPrefsQuestStatesKey + localIdentifier.ToString());

            // TODO: In the future we'd probably want to make a copy of the quest (QuestManager.instance.quests ..) and add that to the player's quest states.
            foreach (var quest in model.activeQuests)
                LoadSerializationModelToSource(quest, QuestManager.instance.quests.FirstOrDefault(o => o.ID == quest.ID), db.activeQuests);

            foreach (var quest in model.completedQuests)
                LoadSerializationModelToSource(quest, QuestManager.instance.quests.FirstOrDefault(o => o.ID == quest.ID), db.completedQuests);

            foreach (var achievement in model.achievements)
                LoadSerializationModelToSource(achievement, QuestManager.instance.achievements.FirstOrDefault(o => o.ID == achievement.ID), db.achievements);


            DevdogLogger.LogVerbose("Deserialized active quests: " + model.activeQuests.Length + " for " + localIdentifier.ToString());
            DevdogLogger.LogVerbose("Deserialized completed quests: " + model.completedQuests.Length + " for " + localIdentifier.ToString());
            DevdogLogger.LogVerbose("Deserialized achievements: " + model.achievements.Length + " for " + localIdentifier.ToString());
        }

        protected virtual QuestsContainerSerializationModel LoadQuestsContainerModel(string key)
        {
            var json = PlayerPrefs.GetString(key, "{}");
            QuestsContainerSerializationModel model = null;
            JsonSerializer.DeserializeTo(ref model, json, null);

            return model;
        }

        protected void LoadSerializationModelToSource<T>(QuestSerializationModel model, T def, ICollection<T> addTo) where T : Quest
        {
            if (def != null)
            {
                model.LoadTo(def);
                addTo.Add(def);
            }
        }
    }
}