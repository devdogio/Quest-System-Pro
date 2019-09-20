namespace Devdog.QuestSystemPro.Dialogue
{
    public interface IEdgeCondition
    {

        /// <summary>
        /// Can the end node this edge is pointing to be seen (viewed by player)?
        /// </summary>
        bool CanViewEndNode(Dialogue dialogue);

        /// <summary>
        /// Can this edge be used? If not it's action will become inactive.
        /// </summary>
        bool CanUse(Dialogue dialogue);


        ValidationInfo Validate(Dialogue dialogue);

        /// <summary>
        /// A short string visualizing the intent of this condition.
        /// </summary>
        string FormattedString();
    }
}