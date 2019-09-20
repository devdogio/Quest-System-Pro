using UnityEngine;
using System.Collections;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;
using Devdog.QuestSystemPro.Dialogue.Editors;
using UnityEditor;

namespace Devdog.General.Editors.GameRules
{
    public class DialogueNodesRule : GameRuleBase
    {
        public override void UpdateIssue()
        {
            var dialogues = UnityEngine.Resources.FindObjectsOfTypeAll<Dialogue>();
            foreach (var dialogue in dialogues)
            {
                foreach (var node in dialogue.nodes)
                {
                    var v = node.Validate();
                    if (v.validationType != ValidationType.Valid)
                    {
                        var d = dialogue;
                        var n = node;
                        issues.Add(new GameRuleIssue(v.message, v.validationType == ValidationType.Warning ? MessageType.Warning : MessageType.Error, new GameRuleAction("Select node",
                            () =>
                            {
                                DialogueEditorWindow.Edit(d);
                                DialogueEditorWindow.FocusOnNode(n);
                                DialogueEditorWindow.PingNode(n);
                            })));
                    }
                }
            }


        }
    }
}