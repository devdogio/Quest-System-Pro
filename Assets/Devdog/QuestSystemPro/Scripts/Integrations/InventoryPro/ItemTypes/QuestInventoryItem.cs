#if INVENTORY_PRO

using System.Collections.Generic;
using Devdog.General;
using Devdog.InventoryPro;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    public class QuestInventoryItem : InventoryItemBase
    {
        [Required]
        public Quest quest;
        public bool useQuestWindow = true;
        public bool removeItemWhenUsed = true;
        
        /// <summary>
        /// When the item is used, play this sound.
        /// </summary>
        public AudioClipInfo audioClipWhenUsed;

        public override LinkedList<ItemInfoRow[]> GetInfo()
        {
            var list = base.GetInfo();

            list.AddFirst(new ItemInfoRow[]{
                new ItemInfoRow("Quest", quest.name.message),
            });

            return list;
        }

        public override void NotifyItemUsed(uint amount, bool alsoNotifyCollection)
        {
            base.NotifyItemUsed(amount, alsoNotifyCollection);

#if !INVENTORY_PRO_LEGACY
            PlayerManager.instance.currentPlayer.inventoryPlayer.stats.SetAll(stats);
#else
            InventoryItemUtility.SetItemProperties(InventoryPlayerManager.instance.currentPlayer, stats, InventoryItemUtility.SetItemPropertiesAction.Use);
#endif
        }

        public override int Use()
        {
            int used = base.Use();
            if (used < 0)
                return used;

            if (currentStackSize <= 0)
                return -2;

            if (quest.CanActivate().status == false)
                return -2;

            // Do something with item
            AudioManager.AudioPlayOneShot(audioClipWhenUsed);

            if (useQuestWindow)
            {
                QuestManager.instance.questWindowUI.acceptCallback.AddListener(ActivateQuest);
                QuestManager.instance.questWindowUI.Repaint(quest);
            }
            else
            {
                if (quest.status == QuestStatus.InActive || quest.status == QuestStatus.Cancelled)
                {
                    ActivateQuest(quest);
                }
            }

            return 1;
        }

        public virtual void ActivateQuest(Quest q)
        {
            q.Activate();

            if (removeItemWhenUsed)
            {
                currentStackSize--; // Remove 1
                NotifyItemUsed(1, true);
            }
        }
    }
}

#endif