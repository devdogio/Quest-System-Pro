using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue.UI;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Will default to the length of the audio clip.Use the additionalWaitTime property to extend the length of the wait time.")]
    [Category("Custom/Flow control")]
    public class WaitForAudioNode : NodeBase
    {
        // Define custom node here.
        public override NodeUIBase uiPrefab
        {
            get
            {
                return null;
            }
        }

        private float waitTime;

        [ShowInNode]
        public float additionalWaitTime;

        [NonSerialized]
        public new string message;


        private static ITimerHelper _timerHelper;


        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            waitTime = 0; // Reset wait time incase the node is being replayed.

            if (_timerHelper == null)
            {
                _timerHelper = TimerUtility.GetTimer();
            }

            if (audioInfo.audioClip.val != null)
            {
                waitTime += audioInfo.audioClip.val.length;
            }
            else
            {
                DevdogLogger.LogError("[WaitForAudioNode] - Audio clip is missing from node: " + index + ". Dialogue: " + dialogueOwner.dialogue.name + ".");
            }

            waitTime += additionalWaitTime;

            _timerHelper.StartTimer(waitTime, null, OnTimerEnded);

            //            Finish(true); // Finish once time is completed.
        }

        protected virtual void OnTimerEnded()
        {
            DevdogLogger.LogVerbose("WaitNode waited " + waitTime + " seconds. Moving to next node.");
            Finish(true);
        }

        public override ValidationInfo Validate()
        {
            if (audioInfo.audioClip.val == null)
            {
                return new ValidationInfo(ValidationType.Error, "[WaitForAudioNode] ERROR - Audio clip is null. This is required.");
            }
            return new ValidationInfo(ValidationType.Valid);
        }
    }
}