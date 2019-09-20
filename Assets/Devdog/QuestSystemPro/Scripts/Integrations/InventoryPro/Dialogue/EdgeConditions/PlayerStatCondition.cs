#if INVENTORY_PRO

using Devdog.General;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro.Dialogue;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    public class PlayerStatCondition : SimpleEdgeCondition
    {
        public StatRequirement statRequirement;

        public override bool CanUse(Dialogue.Dialogue dialogue)
        {
            return statRequirement.CanUse(PlayerManager.instance.currentPlayer.inventoryPlayer);
        }

        public override string FormattedString()
        {
            if(statRequirement == null || statRequirement.stat == null)
            {
                return "(StatRequirement not set)";
            }

            return statRequirement.ToString();
        }
    }
}

#endif