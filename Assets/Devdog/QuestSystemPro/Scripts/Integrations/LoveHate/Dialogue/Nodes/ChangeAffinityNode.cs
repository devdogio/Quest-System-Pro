#if LOVE_HATE

using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using PixelCrushers.LoveHate;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.LoveHate
{
    [System.Serializable]
    [Category("Devdog/Love Hate")]
    public class ChangeAffinityNode : ActionNodeBase
    {
        protected enum ChangeValueType
        {
            Change,
            Set
        }

        [Required]
        [ShowInNode]
        public string judgeFactionName = "Some NPC";

        [Required]
        [ShowInNode]
        public string subjectFactionName = "Player";

        [ShowInNode]
        [Range(-100f, 100f)]
        public float affinity;

        [ShowInNode]
        [SerializeField]
        protected ChangeValueType type = ChangeValueType.Change;


        protected ChangeAffinityNode()
            : base()
        {

        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            switch (type)
            {
                case ChangeValueType.Change:
                    QuestSystemLoveHateBridgeManager.factionManager.ModifyPersonalAffinity(judgeFactionName, subjectFactionName, affinity);
                    break;
                case ChangeValueType.Set:
                    QuestSystemLoveHateBridgeManager.factionManager.SetPersonalAffinity(judgeFactionName, subjectFactionName, affinity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Finish(true);
        }
    }
}

#endif