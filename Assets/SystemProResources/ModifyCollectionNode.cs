using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue;
using Devdog.InventoryPro;

namespace Zingoshi.Game
{
    [Serializable]
    [Category("Custom/ModifyCollectionNode")]
    [Summary("Adds or removes items from a specified collection.")]
    public class ModifyCollectionNode : ActionNodeBase
    {
        [Required]
        [ShowInNode]
        public InventoryItemBase item;

        [ShowInNode]
        public uint amount = 1;

        // Should the specified item be removed?
        [ShowInNode]
        public bool removeItem;

        /// <summary>
        /// OnExecute is called the moment the node is executed.
        /// </summary>
        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            if (removeItem)
            {
                InventoryManager.RemoveItem(item.ID, amount, false);
            }
            else
            {
                item.currentStackSize = amount;
                InventoryManager.AddItem(item);
            }

            // Finish needs to be called to let execution know when the node is completed. False waits for input, true does not.
            Finish(true);
        }
    }
}


