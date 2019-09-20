using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [CreateAssetMenu(menuName = QuestSystemPro.ProductName + "/Quest Database")]
    public partial class QuestDatabase : ScriptableObject
    {
        public Quest[] quests = new Quest[0];
        public Achievement[] achievements = new Achievement[0];
    }
}