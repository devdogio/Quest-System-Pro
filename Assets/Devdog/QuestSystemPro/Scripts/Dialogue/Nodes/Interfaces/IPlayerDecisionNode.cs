namespace Devdog.QuestSystemPro.Dialogue
{
    public interface IPlayerDecisionNode
    {

        int playerDecisionIndex { get; }
        PlayerDecision[] playerDecisions { get; }

        /// <summary>
        /// Set the player's decision index. Note that this will move to the next node automatically.
        /// </summary>
        void SetPlayerDecisionAndMoveToNextNode(int index);


    }
}