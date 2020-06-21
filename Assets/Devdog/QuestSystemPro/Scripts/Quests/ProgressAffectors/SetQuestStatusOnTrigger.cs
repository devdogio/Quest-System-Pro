using System.Collections;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
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
            _onStayWaitForSeconds = new WaitForSeconds(onStayChangeInterval);
        }

        private bool IsValidBehaviour()
        {
            return isActiveAndEnabled && gameObject.activeInHierarchy;
        }

        private bool IsValidTrigger(TriggerType currentType)
        {
            return _triggerType == currentType;
        }

        #region 3D Trigger Behaviour
        private bool IsValidTarget(Collider other)
        {
            if(string.IsNullOrEmpty(onlyWithTag))
            {
                return other.gameObject.GetComponent<Player>() != null;
            }

            return other.CompareTag(onlyWithTag);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!IsValidBehaviour() || !IsValidTarget(other))
            {
                return;
            }

            _playerInTrigger = true;
            if(IsValidTrigger(TriggerType.OnEnter))
            {
                quest.DoAction(action, force);
            }

            if(_triggerType == TriggerType.OnStay)
            {
                StartCoroutine(_OnStay(other));
            }
        }

        private IEnumerator _OnStay(Collider other)
        {
            // Keeps going forever untill StopCoroutine is called.
            while(_playerInTrigger)
            {
                yield return _onStayWaitForSeconds;

                quest.DoAction(action, force);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(!IsValidBehaviour() || !IsValidTarget(other))
            {
                return;
            }

            _playerInTrigger = false;
            if(IsValidTrigger(TriggerType.OnExit))
            {
                quest.DoAction(action, force);
            }
        }
        #endregion

        #region 2D Trigger Behaviour
        private bool IsValidTarget2D(Collider2D collision)
        {
            if(string.IsNullOrEmpty(onlyWithTag))
            {
                return collision.gameObject.GetComponent<Player2D>() != null;
            }

            return collision.CompareTag(onlyWithTag);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!IsValidBehaviour() || !IsValidTarget2D(collision))
            {
                return;
            }

            _playerInTrigger = true;
            if(IsValidTrigger(TriggerType.OnEnter))
            {
                quest.DoAction(action, force);
            }

            if(_triggerType == TriggerType.OnStay)
            {
                StartCoroutine(_OnStay2D(collision));
            }
        }

        private IEnumerator _OnStay2D(Collider2D collision)
        {
            // Keeps going forever untill StopCoroutine is called.
            while(_playerInTrigger)
            {
                yield return _onStayWaitForSeconds;

                quest.DoAction(action, force);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(!IsValidBehaviour() || !IsValidTarget2D(collision))
            {
                return;
            }

            _playerInTrigger = false;
            if(IsValidTrigger(TriggerType.OnExit))
            {
                quest.DoAction(action, force);
            }
        }
        #endregion
    }
}
