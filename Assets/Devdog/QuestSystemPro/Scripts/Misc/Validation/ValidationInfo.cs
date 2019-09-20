namespace Devdog.QuestSystemPro
{
    public struct ValidationInfo
    {
        public ValidationType validationType;
        public string message;

        public ValidationInfo(ValidationType validationType, string message = "")
        {
            this.validationType = validationType;
            this.message = message;
        }
    }
}