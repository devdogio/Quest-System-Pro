using System.Collections;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [RequireComponent(typeof(SphereCollider))]
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Set Quest Progress/Set Quest Status On Trigger")]
    public sealed class SetQuestStatusOnTrigger : MonoBehaviour
    {
        private enum TriggerType
        {
            OnEnter,
            OnStay,
            OnExit
        }

        [Required]
        [ForceCustomObjectPicker]
        public Quest quest;
        public QuestStatusAction action;
        public bool force = false;

        [Header("Trigger configuration")]
        [SerializeField]
        private TriggerType _triggerType;
        public float onStayChangeInterval = 10f;
        public string onlyWithTag = "";

        private bool _playerInTrigger = false;
        private WaitForSeconds _onStayWaitForSeconds;
        private void Awake()
        {
            var col = GetComponent<SphereCollider>();
            col.isTrigger = true;

            _onStayWaitForSeconds = new WaitForSeconds(onStayChangeInterval);
        }

        private bool IsValid(TriggerType currentType)
        {
            return _triggerType == currentType;
        }

        private bool IsValidTarget(Collider other)
        {
            if (string.IsNullOrEmpty(onlyWithTag))
            {
                return other.gameObject.GetComponent<Player>() != null;
            }

            return other.CompareTag(onlyWithTag);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isActiveAndEnabled == false || gameObject.activeInHierarchy == false)
            {
                return;
            }

            if (IsValidTarget(other))
            {
                _playerInTrigger = true;
                if (IsValid(TriggerType.OnEnter))
                {
                    quest.DoAction(action, force);
                }

                if (_triggerType == TriggerType.OnStay)
                {
                    StartCoroutine(_OnStay(other));
                }
            }
        }

        private IEnumerator _OnStay(Collider other)
        {
            // Keeps going forever untill StopCoroutine is called.
            while (_playerInTrigger)
            {
                yield return _onStayWaitForSeconds;

                quest.DoAction(action, force);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isActiveAndEnabled == false || gameObject.activeInHierarchy == false)
            {
                return;
            }

            if (IsValidTarget(other))
            {
                _playerInTrigger = false;
                if (IsValid(TriggerType.OnExit))
                {
                    quest.DoAction(action, force);
                }
            }
        }
    }
}
