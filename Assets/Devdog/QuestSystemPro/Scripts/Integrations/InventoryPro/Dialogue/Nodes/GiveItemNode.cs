#if INVENTORY_PRO

using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro.Dialogue;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    [System.Serializable]
    [Category("Devdog/Inventory Pro")]
    public class GiveItemNode : ActionNodeBase
    {
        /// <summary>
        /// The items the player receives when this node is executed.
        /// </summary>
        [ShowInNode]
        [HideGroup]
        public ItemAmountRow[] items;


        protected ItemAmountRow[] GetRows()
        {
            return items.Select(o => new ItemAmountRow(o.item, o.amount)).ToArray();
        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            var inventoryItems = GetRows();
            if (InventoryManager.CanAddItems(inventoryItems))
            {
                foreach (var item in inventoryItems)
                {
                    var i = UnityEngine.Object.Instantiate<InventoryItemBase>(item.item);
                    i.currentStackSize = item.amount;
                    InventoryManager.AddItem(i);
                }

                Finish(true);
                return;
            }

            Failed(QuestManager.instance.languageDatabase.inventoryIsFull); // Couldn't add items
        }
    }
}

#endif