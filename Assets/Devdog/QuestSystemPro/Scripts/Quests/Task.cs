using System;
using Devdog.General;
using Devdog.General.Localization;
using Devdog.QuestSystemPro.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro
{
    [System.Serializable]
    public partial class Task
    {
        public delegate void ReachedTimeLimit(Task task);
        public delegate void StatusChanged(TaskStatus before, TaskStatus after, Task self);
        public delegate void ProgressChanged(float before, Task task);

        public event ReachedTimeLimit OnReachedTimeLimit;
        public event ProgressChanged OnProgressChanged;
        public event StatusChanged OnStatusChanged;

        public virtual TaskProgressRowUI taskUIPrefab
        {
            get { return QuestManager.instance.settingsDatabase.defaultTaskRowUI; }
        }

        public string key;

        [TextArea]
        public LocalizedString description = new LocalizedString();
        public LocalizedString statusMessage = new LocalizedString(); //  { message = "Captured {0}/{2} items" }

        [SerializeField]
        protected Sprite _icon;
        public virtual Sprite icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        [SerializeField]
        private float _progressCap = 1f;
        public float progressCap
        {
            get { return _progressCap; }
            protected set { _progressCap = value; }
        }

        /// <summary>
        /// Auto complete this task when there's enough progress to do so.
        /// When false SetCompleted() has to be called manually.
        /// </summary>
        public bool autoComplete = false;


        [NonSerialized]
        private float _progress;
        public float progress
        {
            get { return _progress; }
        }

        public float progressNormalized
        {
            get { return progress/progressCap; }
        }


        [Header("Limits")]
        public bool useTimeLimit = false;
        public float timeLimitInSeconds = 300f;

        public double timeSinceStartInSeconds
        {
            get
            {
                if (startTime == null)
                {
                    return 0d;
                }

                return (DateTime.Now - startTime).Value.TotalSeconds;
            }
        }

        public double timeSinceStartNormalized
        {
            get { return timeSinceStartInSeconds / timeLimitInSeconds; }
        }

        [NonSerialized]
        protected int timerID = -1;

        [NonSerialized]
        private DateTime? _startTime;
        public DateTime? startTime
        {
            get { return _startTime; }
            protected set { _startTime = value; }
        }

        [NonSerialized]
        protected ITimerHelper timer;

        public TaskRequirement requirement = TaskRequirement.Required;
        public bool giveRewardsOnTaskComplete = false;

        public IRewardGiver[] rewardGivers = new IRewardGiver[0];

        [NonSerialized]
        private TaskStatus _status = TaskStatus.InActive;
        public TaskStatus status
        {
            get { return _status; }
            protected set
            {
                var before = _status;
                _status = value;

                if (before != _status)
                {
                    if(OnStatusChanged != null)
                    {
                        OnStatusChanged(before, _status, this);
                    }
                }
            }
        }
        
        public bool isCompleted
        {
            get { return status == TaskStatus.Completed; }
        }

        /// <summary>
        /// When true the user can overachieve tasks extra rewards can be given for extra effort.
        /// Example quest: 
        /// Gather 5 apples for 10 XP
        /// > User gathers 10 apples
        /// User receives 10XP + (bonus)
        /// 
        /// <b>The rewards of the over achievement are combined with the rewards from the task.</b>
        /// </summary>
        public TaskOverAchievement[] overAchievements = new TaskOverAchievement[0];


        [NonSerialized]
        private bool _gaveRewards;
        public bool gaveRewards
        {
            get { return _gaveRewards; }
            protected set { _gaveRewards = value; }
        }


        /// <summary>
        /// The owner of this task (quest). Assigned at run-time.
        /// </summary>
        [IgnoreCustomSerialization]
        public virtual Quest owner { get; set; }


//        [Obsolete("Use other constructors instead.")]
        public Task()
        { }

        public Task(string key, float progressCap)
            : this(key, progressCap, null)
        { }

        public Task(string key, float progressCap, params IRewardGiver[] rewardGivers)
        {
            this.key = key;
            this.progressCap = progressCap;
            this.rewardGivers = rewardGivers ?? new IRewardGiver[0];
        }

        public bool ChangeProgress(float amount)
        {
            return SetProgress(progress + amount);
        }

        public virtual bool CanSetProgress(float amount)
        {
            return owner.CanSetTaskProgress(key, amount);
        }

        public virtual bool SetProgress(float amount)
        {
            var before = _progress;
            _progress = amount;

            if (Mathf.Approximately(before, _progress) == false)
            {
                if (OnProgressChanged != null)
                {
                    OnProgressChanged(before, this);
                }
            }

            if (autoComplete && IsProgressSufficientToComplete())
            {
                Complete();
            }

            return true;
        }

        protected void NotifyTimerStarted()
        {
            owner.timeHandler.OnTimerStarted(this);
        }

        protected void NotifyTimerUpdated()
        {
            owner.timeHandler.OnTimerUpdated(this);
        }

        protected void NotifyTimerStopped()
        {
            owner.timeHandler.OnTimerStopped(this);
        }

        protected void NotifyReachedTimeLimit()
        {
            if (OnReachedTimeLimit != null)
            {
                OnReachedTimeLimit(this);
            }

            owner.timeHandler.OnReachedTimeLimit(this);
        }

        public void StartTimer()
        {
            if (timer == null)
            {
                timer = TimerUtility.GetTimer();
            }

            StopTimer();
            startTime = DateTime.Now;

            NotifyTimerStarted();
            timerID = timer.StartTimer(timeLimitInSeconds, NotifyTimerUpdated, NotifyReachedTimeLimit);
        }

        public void StopTimer()
        {
            startTime = null;

            if (timerID != -1)
            {
                NotifyTimerStopped();
                timer.StopTimer(timerID);

                timerID = -1;
            }
        }

        public virtual void Activate()
        {
            if (status == TaskStatus.InActive)
            {
                startTime = DateTime.Now;
                if (useTimeLimit)
                {
                    StartTimer();
                }
                status = TaskStatus.Active;
            }
            else if (status == TaskStatus.Completed)
            {
                DevdogLogger.Log("Trying to activate completed task. If you wish to start it again call SetProgress instead.");
            }
        }

        public virtual ConditionInfo CanComplete()
        {
            if (gaveRewards == false)
            {
                var s = CanGiveRewards();
                if (s == false)
                {
                    return s;
                }
            }

            if (IsProgressSufficientToComplete() == false)
            {
                return new ConditionInfo(false, QuestManager.instance.languageDatabase.questTaskNotCompleted, key);
            }

            if (useTimeLimit)
            {
                if (timeSinceStartNormalized > 1f)
                {
                    return new ConditionInfo(false, QuestManager.instance.languageDatabase.canNotCompleteQuestOverTimeLimit);
                }
            }

            return ConditionInfo.success;
        }

        public virtual bool Complete(bool forceComplete = false)
        {
            if (CanComplete() == false && forceComplete == false)
            {
                return false;
            }

            status = TaskStatus.Completed;
            StopTimer();

            if (giveRewardsOnTaskComplete)
            {
                GiveRewards();
            }

            DevdogLogger.LogVerbose("Completed task: \"" + key + "\" on " + owner.GetType().Name + " \"" + owner.name + "\"");
            return isCompleted;
        }

        public virtual void NotifyQuestCompleted()
        {
            
//            Assert.AreEqual(QuestStatus.Completed, owner.status, "Quest status is not completed, yet Notify is called!");
        }

        public virtual void Fail()
        {
            if (status != TaskStatus.Completed)
            {
                status = TaskStatus.Failed;
                StopTimer();
            }
            else
            {
                DevdogLogger.Log("Trying to fail completed task. If you wish to start it again call SetProgress instead.");
            }
        }

        public virtual ConditionInfo CanGiveRewards()
        {
            if (gaveRewards)
            {
                return new ConditionInfo(false);
            }

            foreach (var rewardGiver in rewardGivers)
            {
                var s = rewardGiver.CanGiveRewards(owner);
                if (s == false)
                {
                    return s;
                }
            }

            var overAchievement = GetCurrentOverAchievement();
            if (overAchievement != null)
            {
                foreach (var rewardGiver in overAchievement.rewardGivers)
                {
                    var s = rewardGiver.CanGiveRewards(owner);
                    if (s == false)
                    {
                        return s;
                    }
                }
            }

            return ConditionInfo.success;
        }

        public virtual bool GiveRewards(bool force = false)
        {
            if (CanGiveRewards() == false && force == false)
            {
                return false;
            }

            gaveRewards = true;
            foreach (var rewardGiver in rewardGivers)
            {
                rewardGiver.GiveRewards(owner);
            }

            var overAchievement = GetCurrentOverAchievement();
            if (overAchievement != null)
            {
                foreach (var rewardGiver in overAchievement.rewardGivers)
                {
                    rewardGiver.GiveRewards(owner);
                }
            }

            return true;
        }

        public virtual void Cancel()
        {
            ResetProgress();
        }

        public bool IsOverAchieved()
        {
            return GetCurrentOverAchievement() != null;
        }

        public virtual TaskOverAchievement GetCurrentOverAchievement()
        {
            if (progressNormalized >= 1f)
            {
                foreach (var overAchievement in overAchievements)
                {
                    if (progress >= overAchievement.from && progress < overAchievement.to)
                    {
                        return overAchievement;
                    }
                }
            }

            return null;
        }

        public virtual bool IsProgressSufficientToComplete()
        {
            return progress >= progressCap;
        }

        public virtual void ResetProgress()
        {
            gaveRewards = false;
            SetProgress(0f);
            StopTimer();
            status = TaskStatus.InActive;
        }

        public virtual string GetStatusMessage()
        {
            try
            {
                return string.Format(statusMessage.message, progress, progressNormalized, progressCap);
            }
            catch (Exception e)
            {
                DevdogLogger.LogError("Couldn't format quest #" + owner.ID + " status message with error: " + e.Message);
            }

            return "";
        }
    }
}