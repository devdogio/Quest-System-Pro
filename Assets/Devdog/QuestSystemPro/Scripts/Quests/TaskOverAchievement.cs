using System;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public partial class TaskOverAchievement
    {
        /// <summary>
        /// The normalized value. From 1f is 100% achievement. From 1.1f = 110% achievement, aka 10% over achievement).
        /// </summary>
        public float from = 1f;

        /// <summary>
        /// The normalized value. From 1f is 100% achievement. From 1.1f = 110% achievement, aka 10% over achievement).
        /// </summary>
        public float to = 1f;

        public float difference
        {
            get { return to - from; }
        }

        public IRewardGiver[] rewardGivers;

//        [Obsolete("Use other constructors instead.", true)]
        public TaskOverAchievement()
        { }

        public TaskOverAchievement(float from, float to, params IRewardGiver[] rewardGivers)
        {
            this.from = from;
            this.to = to;
            this.rewardGivers = rewardGivers ?? new IRewardGiver[0];
        }
    }
}