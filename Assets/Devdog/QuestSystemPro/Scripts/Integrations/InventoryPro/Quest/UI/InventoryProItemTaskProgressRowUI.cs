#if INVENTORY_PRO

using Devdog.InventoryPro;
using Devdog.QuestSystemPro.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.InventoryPro.UI
{
    public class InventoryProItemTaskProgressRowUI : TaskProgressRowUI
    {
        [Header("Inventory Pro UI Reference")]
#if !INVENTORY_PRO_LEGACY
        public ItemCollectionSlotUIBase wrapper;
#else
        public InventoryUIItemWrapper wrapper;
#endif

        public override void Repaint(Task task)
        {
            base.Repaint(task);

            var inventoryTask = (IInventoryProTask)task;
            var item = inventoryTask.item;

            if (wrapper != null)
            {
                wrapper.item = item;
                wrapper.Repaint();
            }
        }
    }
}

#endif