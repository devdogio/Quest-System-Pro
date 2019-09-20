using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue.UI
{
    public class DefaultTimedNodeUI : DefaultNodeUI
    {
        [Header("Options")]
        public float waitTimePerLetter = 0.1f;
        public bool showDefaultPlayerDecisions = true;
        public bool stopAtLeafNode = true;

        protected override void SetText(string msg)
        {
            base.SetText(msg);

            var currentNodeTemp = currentNode;
            if (msg.Length > 0)
            {
                TimerUtility.GetTimer().StartTimer(msg.Length * waitTimePerLetter, () =>
                {
                    if (stopAtLeafNode && currentNodeTemp.isLeafNode)
                    {
                        return;
                    }

                    currentNodeTemp.Finish(true);
                });
            }
        }

        protected override void SetDefaultPlayerDecision()
        {
            if (showDefaultPlayerDecisions)
            {
                base.SetDefaultPlayerDecision();
            }
        }
    }
}