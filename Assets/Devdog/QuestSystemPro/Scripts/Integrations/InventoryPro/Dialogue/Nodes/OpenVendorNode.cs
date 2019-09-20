#if INVENTORY_PRO

using System;
using Devdog.General;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro.Dialogue;

namespace Devdog.QuestSystemPro.Integration.InventoryPro
{
    [System.Serializable]
    [Category("Devdog/Inventory Pro")]
    public class OpenVendorNode : Node
    {
        [NonSerialized]
        public new string message;


        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            if (DialogueManager.instance.currentDialogueOwner != null && DialogueManager.instance.currentDialogueOwner.transform != null)
            {
                var trigger = DialogueManager.instance.currentDialogueOwner.transform.GetComponent<VendorTrigger>();
                if (trigger != null)
                {
#if !INVENTORY_PRO_LEGACY
                    trigger.OnTriggerUsed(PlayerManager.instance.currentPlayer);
#else
                    trigger.OnTriggerUsed(InventoryPlayerManager.instance.currentPlayer);
#endif
                }
            }

            Finish(true);
        }
    }
}

#endif