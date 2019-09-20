using UnityEngine;
using System.Collections;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.UI;

public class MyRewardGiver : INamedRewardGiver
{
    [SerializeField]
    private string _name;
    public string name
    {
        get { return _name; }
    }

    public RewardRowUI rewardUIPrefab
    {
        get { return QuestManager.instance.settingsDatabase.defaultRewardRowUI; }
    }

    public ConditionInfo CanGiveRewards(Quest quest)
    {
        return ConditionInfo.success;
    }

    public void GiveRewards(Quest quest)
    {
        Debug.Log("Give rewards for quest: " + quest.name);
    }

    public override string ToString()
    {
        return "Debug log message!";
    }
}
