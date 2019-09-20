#if INVENTORY_PRO

using Devdog.InventoryPro;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    public interface IInventoryProTask
    {
        InventoryItemBase item { get; }
    }
}

#endif