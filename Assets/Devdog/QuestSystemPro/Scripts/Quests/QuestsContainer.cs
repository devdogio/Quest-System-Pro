using System.Collections.Generic;

namespace Devdog.QuestSystemPro
{
    public partial class QuestsContainer 
    {
        public HashSet<Quest> completedQuests = new HashSet<Quest>();
        public HashSet<Quest> activeQuests = new HashSet<Quest>();

        /// <summary>
        /// All achievemetns with any type of progress.
        /// </summary>
        public HashSet<Achievement> achievements = new HashSet<Achievement>(); 
    }
}