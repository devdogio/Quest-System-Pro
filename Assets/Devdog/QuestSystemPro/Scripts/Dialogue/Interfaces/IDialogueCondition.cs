using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    public interface IDialogueCondition
    {

        DialogueConditionInfo CanDiscover(Dialogue dialogue, ILocalIdentifier identifier);
        DialogueConditionInfo CanStart(Dialogue dialogue, ILocalIdentifier identifier);
        DialogueConditionInfo CanStop(Dialogue dialogue, ILocalIdentifier identifier);

    }
}