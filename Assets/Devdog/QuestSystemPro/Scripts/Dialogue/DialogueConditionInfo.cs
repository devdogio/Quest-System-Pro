namespace Devdog.QuestSystemPro.Dialogue
{
    public struct DialogueConditionInfo
    {
        public bool status;
        public string message;

        public DialogueConditionInfo(bool conditionStatus, string conditionMessage = "")
        {
            status = conditionStatus;
            message = conditionMessage;
        }
    }
}