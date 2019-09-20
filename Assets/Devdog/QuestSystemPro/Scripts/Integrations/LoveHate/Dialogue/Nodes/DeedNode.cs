#if LOVE_HATE

using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using PixelCrushers;
using PixelCrushers.LoveHate;

namespace Devdog.QuestSystemPro.Integration.LoveHate
{
    [System.Serializable]
    [Category("Devdog/Love Hate")]
    public class DeedNode : ActionNodeBase
    {
        [ShowInNode]
        [HideTypePicker]
        [HideGroup]
        public DeedOverrideInfo deedInfo;

        public bool requiresSight = false;
        public Dimension dimension = Dimension.Is3D;
        public float radius = 0f;

        protected DeedNode()
            : base()
        {

        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
//            var factionManager = QuestSystemLoveHateBridgeManager.factionManager;
            FactionMember factionMember = null;
            switch (ownerType)
            {
                case DialogueOwnerType.Player:
                {
                    var p = PlayerManager.instance.currentPlayer;
                    factionMember = p.GetComponent<FactionMember>();
                    break;
                }
                case DialogueOwnerType.DialogueOwner:
                {
                    var qg = QuestManager.instance.currentQuestGiver;
                    if (qg != null)
                    {
                        factionMember = qg.transform.GetComponent<FactionMember>();
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (factionMember != null)
            {
                var deed = Deed.GetNew(deedInfo.tag, factionMember.factionID, deedInfo.targetFactionID, deedInfo.impact, deedInfo.aggression, factionMember.GetPowerLevel(), deedInfo.traits);

//                int numPersonalityTraits = factionManager.factionDatabase.personalityTraitDefinitions.Length;
//                float[] traits = Traits.Allocate(numPersonalityTraits, true); // Optional values that describe personality of deed.

                QuestSystemLoveHateBridgeManager.factionManager.CommitDeed
                    (
                        factionMember,
                        deed,
                        requiresSight,
                        dimension,
                        radius
                    );

                Deed.Release(deed);
            }

            Finish(true);
        }
    }
}

#endif