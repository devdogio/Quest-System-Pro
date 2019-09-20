#if INVENTORY_PRO

using Devdog.General;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro.Dialogue;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    public class HasItemInInventory : SimpleEdgeCondition
    {
        public enum FilterType
        {
            Equal,
            NotEqual,
            GreaterThanOrEqual,
            LessThanOrEqual
        }

        [Required]
        public InventoryItemBase item;

        public int amount = 1;
        public FilterType filterType = FilterType.GreaterThanOrEqual;

        public override bool CanUse(Dialogue.Dialogue dialogue)
        {
            return AbidesFilter();
        }

        public override string FormattedString()
        {
            if(item == null)
            {
                return "(No item set)";
            }

            if (AbidesFilter())
            {
                return "Has item " + item.name;
            }

            return "Does not have item " + item.name;
        }

        protected bool AbidesFilter()
        {
            var count = InventoryManager.GetItemCountLike(item, false);
            switch (filterType)
            {
                case FilterType.Equal:
                    return count == amount;
                case FilterType.NotEqual:
                    return count != amount;
                case FilterType.GreaterThanOrEqual:
                    return count >= amount;
                case FilterType.LessThanOrEqual:
                    return count <= amount;
            }

            return false;
        }
    }
}

#endif