using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [RequireComponent(typeof(Animator))]
    public sealed class SpawnerObjectAnimator : MonoBehaviour, ISpawnerObjectCallbacks
    {
        [Required]
        public AnimationClip inAnimation;

        private Animator _animator;
        private int _hash;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _hash = Animator.StringToHash(inAnimation.name);
        }

        public void OnSpawned(SpawnerBase spawner, SpawnerCategoryInfo category)
        {
            _animator.Play(_hash);
        }
    }
}
