namespace Devdog.QuestSystemPro
{
    public interface INamedRewardGiver : IRewardGiver
    {
        string name { get; }

        string ToString();
    }
}