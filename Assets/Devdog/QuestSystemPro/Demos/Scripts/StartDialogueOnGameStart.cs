using UnityEngine;
using System.Collections;
using Devdog.General;

namespace Devdog.QuestSystemPro.Demo
{
    public class StartDialogueOnGameStart : MonoBehaviour
    {
        [Required]
        public Dialogue.Dialogue dialogue;

        protected void Start()
        {
            dialogue.StartDialogue();
        }
    }
}