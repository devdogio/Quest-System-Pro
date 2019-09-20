using System.Collections;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [RequireComponent(typeof(SphereCollider))]
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Set Quest Progress/Set Quest Progress On Trigger")]
    public sealed class SetQuestProgressOnTrigger : MonoBehaviour
    {
        private enum TriggerType
        {
            OnEnter,
            OnStay,
            OnExit
        }

        public QuestProgressDecorator progress;

        [Header("Trigger configuration")]
        [SerializeField]
        private TriggerType _triggerType;
        public float onStayChangeInterval = 10f;
        public string onlyWithTag = "";

        private bool _rewarded = false;
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
            return _rewarded == false &&
                _triggerType == currentType;
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
                    _rewarded = progress.Execute();
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

                var r = progress.Execute();
                if (r)
                {
                    _rewarded = true;
                }
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
                    _rewarded = progress.Execute();
                }
            }
        }
    }
}
