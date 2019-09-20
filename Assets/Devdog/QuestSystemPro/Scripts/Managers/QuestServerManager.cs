using Devdog.General;
using UnityEngine;
using UnityEngine.Networking;

namespace Devdog.QuestSystemPro
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Managers/Multiplayer/Quest Server Manager")]
    public partial class QuestServerManager : QuestManager
    {
        protected override void Awake()
        {
            base.Awake();
        }


        protected ILocalIdentifier GetLocalIdentifierFromNetworkIdentity(NetworkIdentity identity)
        {
            foreach (var q in questStates)
            {
                if (q.Key.ID == identity.playerControllerId.ToString()) // TODO: Not a valid ID comparison
                {
                    return q.Key;
                }
            }

            DevdogLogger.LogWarning("Local identifier not found for identity with controller ID: " + identity.playerControllerId);
            return null;
        }


        public override bool HasCompletedQuest(Quest quest)
        {
            DevdogLogger.LogVerbose("Checking server ... ");
            CmdHasCompletedQuest(quest);

            bool completed = questStates[quest.localIdentifier].completedQuests.Contains(quest);
            return completed;
        }

        // TODO: Move commands to a different script that inherits from NetworkBehaviour

        protected void CmdHasCompletedQuest(Quest quest)
        {
            bool completed = questStates[quest.localIdentifier].completedQuests.Contains(quest);
            DevdogLogger.LogVerbose("(Server) Has " + quest.localIdentifier.ID + " completed quest: " + completed);
        }

        public override void NotifyQuestTaskStatusChanged(TaskStatus before, TaskStatus after, Task task, Quest quest)
        {
            _NotifyQuestTaskStatusChanged(before, after, task, quest);
        }

        protected void _NotifyQuestTaskStatusChanged(TaskStatus before, TaskStatus after, Task task, Quest quest)
        {
            DevdogLogger.LogVerbose("(Server) Quest task " + task.key + " status changed to " + task.isCompleted);
        }

        public override void NotifyQuestTaskProgressChanged(float before, Task task, Quest quest)
        {
            _NotifyQuestTaskProgressChanged(before, task, quest);
        }

        protected void _NotifyQuestTaskProgressChanged(float before, Task task, Quest quest)
        {
            DevdogLogger.LogVerbose("(Server) Quest task " + task.key + " progress changed to " + task.progress);
        }

        public override void NotifyQuestStatusChanged(QuestStatus before, Quest quest)
        {
            _NotifyQuestStatusChanged(before, quest);
        }

        protected void _NotifyQuestStatusChanged(QuestStatus before, Quest quest)
        {
            DevdogLogger.LogVerbose("(Server) Quest " + quest.name + " status changed to " + quest.status);

            if (quest.status == QuestStatus.Completed)
            {
                questStates[quest.localIdentifier].completedQuests.Add(quest);
                DevdogLogger.LogVerbose("(Server) Completed quest " + quest.name);
            }
        }

        public override void NotifyAchievementTaskStatusChanged(TaskStatus before, TaskStatus after, Task task, Achievement achievement)
        {
            _NotifyAchievementTaskStatusChanged(before, after, task, achievement);
        }

        protected void _NotifyAchievementTaskStatusChanged(TaskStatus before, TaskStatus after, Task task, Achievement achievement)
        {
            DevdogLogger.LogVerbose("(Server) achievement task " + task.key + " status changed to " + task.isCompleted);
        }

        public override void NotifyAchievementTaskProgressChanged(float before, Task task, Achievement achievement)
        {
            _NotifyAchievementTaskProgressChanged(before, task, achievement);
        }

        protected void _NotifyAchievementTaskProgressChanged(float before, Task task, Achievement achievement)
        {
            DevdogLogger.LogVerbose("(Server) achievement task " + task.key + " progress changed to " + task.progress);
        }

        public override void NotifyAchievementStatusChanged(QuestStatus before, Achievement achievement)
        {
            _NotifyAchievementStatusChanged(before, achievement);
        }

        protected void _NotifyAchievementStatusChanged(QuestStatus before, Achievement achievement)
        {
            DevdogLogger.LogVerbose("(Server) achievement " + achievement.name + " status changed to " + achievement.status);
        }


#if UNITY_EDITOR
        public override void Reset()
        {
            Awake();

            var manager = FindObjectOfType<NetworkManager>();
            if (manager.isNetworkActive == false)
            {
                manager.StartHost();
            }

//            manager.StopHost();
        }
#endif
    }
}