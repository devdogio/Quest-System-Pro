using Devdog.General;
using System.Collections;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
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
            _onStayWaitForSeconds = new WaitForSeconds(onStayChangeInterval);
        }

        private bool IsValidBehaviour()
        {
            return isActiveAndEnabled && gameObject.activeInHierarchy;
        }

        private bool IsValidTrigger(TriggerType currentType)
        {
            return _rewarded == false &&
                _triggerType == currentType;
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
                _rewarded = progress.Execute();
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

                var r = progress.Execute();
                if(r)
                {
                    _rewarded = true;
                }
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
                _rewarded = progress.Execute();
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
                _rewarded = progress.Execute();
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

                var r = progress.Execute();
                if(r)
                {
                    _rewarded = true;
                }
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
                _rewarded = progress.Execute();
            }
        }
        #endregion
    }
}
