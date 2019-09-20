namespace Devdog.QuestSystemPro.Dialogue
{
    public abstract class SimpleEdgeCondition : IEdgeCondition
    {
        public bool canViewEndNode = false;

        public virtual bool CanViewEndNode(Dialogue dialogue)
        {
            return CanUse(dialogue) || canViewEndNode;
        }

        public virtual ValidationInfo Validate(Dialogue dialogue)
        {
            return new ValidationInfo(ValidationType.Valid);
        }

        public abstract bool CanUse(Dialogue dialogue);
        public abstract string FormattedString();
    }
}
