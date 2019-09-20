using Devdog.General;
using Devdog.General.UI;

namespace Devdog.QuestSystemPro.Dialogue.UI
{
    public class PlayerDecisionNodeUI : DefaultNodeUI
    {
        public PlayerDecisionUI playerDecisionUIPrefab;

        private PlayerDecision[] _decisions;
        protected override void SetDecisions()
        {
            var decisionNode = (IPlayerDecisionNode)currentNode;
            _decisions = decisionNode.playerDecisions;

            for (int i = 0; i < decisionNode.playerDecisions.Length; i++)
            {
                var decision = decisionNode.playerDecisions[i];
                if (i >= currentNode.edges.Length)
                {
                    continue;
                }
                
                if (currentNode.edges[i].CanViewEndNode(currentNode.owner) == false)
                {
                    continue;
                }

                var playerDecisionInst = Instantiate<PlayerDecisionUI>(playerDecisionUIPrefab);
                playerDecisionInst.transform.SetParent(playerDecisionsContainer);
                UIUtility.ResetTransform(playerDecisionInst.transform);
                
                playerDecisionInst.Repaint(decision, currentNode.edges[i], currentNode.edges[i].CanUse(currentNode.owner));

                var tempIndex = i;
                playerDecisionInst.button.onClick.AddListener(() =>
                {
                    OnPlayerDecisionClicked(tempIndex);
                });

                decisions.Add(playerDecisionInst);
            }

            // No defined decisions, set default
            if (decisionNode.playerDecisions.Length == 0)
            {
                SetDefaultPlayerDecision();
            }
        }

        protected virtual void OnPlayerDecisionClicked(int decisionIndex)
        {
            var decisionNode = (IPlayerDecisionNode)currentNode;
            if (ReferenceEquals(_decisions, decisionNode.playerDecisions) == false)
            {
                DevdogLogger.Log("Player decisions changed during repaint and user click - Forcing repaint.");
                Repaint(currentNode); // Force repaint
                return;
            }

            decisionNode.SetPlayerDecisionAndMoveToNextNode(decisionIndex);
        }
    }
}