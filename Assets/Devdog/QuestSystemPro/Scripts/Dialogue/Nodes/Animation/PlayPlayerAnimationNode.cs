using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Play an animation on the player object.")]
    [Category("Devdog/Animation")]
    public class PlayPlayerAnimationNode : ActionNodeBase
    {
        [ShowInNode]
        public AnimationClip animation;

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            var animator = PlayerManager.instance.currentPlayer.gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(animation.name);
            }

            Finish(true);
        }

        public override ValidationInfo Validate()
        {
            if (animation == null)
            {
                return new ValidationInfo(ValidationType.Error, "Animation field is empty");
            }

            return base.Validate();
        }
    }
}