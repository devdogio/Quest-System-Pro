using Devdog.General;
using Devdog.QuestSystemPro.Dialogue.UI;
using Devdog.QuestSystemPro.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [CreateAssetMenu(menuName = QuestSystemPro.ProductName + "/Settings Database")]
    public partial class SettingsDatabase : ScriptableObject
    {
        [Category("Quests"), Header("Quests")]
        public int playerMaxActiveQuests = 10;

        [Category("UI Prefabs"), Header("UI Prefabs")]
        public RewardRowUI defaultRewardRowUI;
        public TaskProgressRowUI defaultTaskRowUI;

        [Header("Node UI Prefabs")]
        public DefaultNodeUI defaultNodeUIPrefab;
        public DefaultNodeUI playerDecisionNodeUI;
        public DefaultNodeUI playerInputNodeUIPrefab;

        [Header("Dialogue UI")]
        public string playerName;
        public Sprite playerDialogueIcon;

    }
}