using System;
using System.Collections.Generic;
using System.Linq;
using Devdog.General.Editors.GameRules;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Editors
{
    public class QuestGiverRule : GameRuleBase
    {
        public override void UpdateIssue()
        {
            var questGivers = UnityEngine.Object.FindObjectsOfType<UnityEngine.Component>().Where(o => o is IQuestGiver);
            foreach (var questGiver in questGivers)
            {
                var iQuestGiver = (IQuestGiver) questGiver;
                if (iQuestGiver.quests.Any(o => o == null))
                {
                    var iQuestGiverTemp = iQuestGiver;
                    var qTemp = questGiver;
                    issues.Add(new GameRuleIssue("Quest giver contains an empty quest", MessageType.Error, new GameRuleAction("Fix (remove)",
                    () =>
                        {
                            iQuestGiverTemp.quests = RemoveNullFromArray(iQuestGiverTemp.quests, qTemp);
                        }
                    ), new GameRuleAction("Select", () =>
                    {
                        SelectObject(qTemp);
                    })));
                }
            }
        }
       
    }
}
