namespace Devdog.QuestSystemPro.Dialogue
{
    public interface IPlayerInputNode
    {

        void SetPlayerInputStringAndMoveToNextNode(string enteredString);
        bool IsInputCorrect(string enteredString);

    }
}