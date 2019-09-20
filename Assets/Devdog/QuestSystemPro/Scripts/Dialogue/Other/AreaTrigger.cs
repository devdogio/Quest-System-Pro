using Devdog.General;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Dialogue
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Dialogue Triggers/Area Trigger")]
    [RequireComponent(typeof(Collider))]
    public sealed class AreaTrigger : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private Trigger _trigger;

        [SerializeField]
        private bool _unUseWhenExitTrigger;

        private void Awake()
        {
            InitTrigger();
        }

        private void OnValidate()
        {
            InitTrigger();
        }

        private void InitTrigger()
        {
            var col = GetComponent<Collider>();
            Assert.IsNotNull(col, "No collider found on AreaTrigger! This is required.");

            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInChildren<Player>() != null)
                _trigger.GetComponent<Trigger>().Use();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponentInChildren<Player>() != null)
                _trigger.GetComponent<Trigger>().Use();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInChildren<Player>() != null)
                if (_unUseWhenExitTrigger)
                    _trigger.GetComponent<Trigger>().UnUse();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponentInChildren<Player>() != null)
                if (_unUseWhenExitTrigger)
                    _trigger.GetComponent<Trigger>().UnUse();
        }
    }
}
