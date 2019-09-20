using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    /// <summary>
    /// Animates an object when the quest status is set.
    /// </summary>
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Quest Object Affectors/Object Animator")]
    public sealed class QuestStatusAnimator : QuestStatusObjectBase
    {
        [Header("Animator")]
        public string animatorStateNameCorrect;
        public string animatorStateNameInCorrect;

        [SerializeField]
        [Required]
        private Animator _animator;

        [Header("Options")]
        public bool useCrossFade = false;
        public float crossFadeDuration = 0.3f;

        [Tooltip("Animate to the correct or incorrect state on game start?\nWhen false no state will be triggered.")]
        public bool syncStateOnStart = false;

        private int _animatorStateNameCorrectHash;
        private int _animatorStateNameInCorrectHash;

        protected override void Awake()
        {
            base.Awake();
            questStatus.syncStateOnCallbackRegistration = syncStateOnStart;

            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }

            _animatorStateNameCorrectHash = Animator.StringToHash(animatorStateNameCorrect);
            _animatorStateNameInCorrectHash = Animator.StringToHash(animatorStateNameInCorrect);
        }

        protected override void OnStatusChangedCorrect(Quest self)
        {
            DoAnimation(_animatorStateNameCorrectHash);
        }

        protected override void OnStatusChangedInCorrect(Quest self)
        {
            DoAnimation(_animatorStateNameInCorrectHash);
        }

        private void DoAnimation(int stateHash)
        {
            if(stateHash != 0)
            {
                if (useCrossFade)
                {
                    _animator.CrossFade(stateHash, crossFadeDuration);
                }
                else
                {
                    _animator.Play(stateHash);
                }
            }
        }
    }
}
