using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue.UI;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Wait for a specified amount of time before continueing.")]
    [Category("Devdog/Flow control")]
    public class WaitNode : NodeBase
    {
        public override NodeUIBase uiPrefab
        {
            get { return null; }
        }

        [ShowInNode]
        public float waitTime;

        [NonSerialized]
        public new string message;


        private static ITimerHelper _timerHelper;


        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            if (_timerHelper == null)
            {
                _timerHelper = TimerUtility.GetTimer();
            }

            _timerHelper.StartTimer(waitTime, null, OnTimerEnded);

//            Finish(true); // Finish once time is completed.
        }

        protected virtual void OnTimerEnded()
        {
            DevdogLogger.LogVerbose("WaitNode waited " + waitTime + " seconds. Moving to next node.");
            Finish(true);
        }
    }
}