using System;
using System.Collections;
using Devdog.General;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.QuestSystemPro.UI
{
    [RequireComponent(typeof(Text))]
    public class TextAnimator : MonoBehaviour, ITextAnimator
    {
        public enum AnimationType
        {
            LetterStep,
            WordStep,
            FadeIn
        }

        public AnimationType animationType;
        public float animationSpeed = 1f;

        [Header("Audio & Visuals")]
        public AudioClipInfo[] stepAudioClips = new AudioClipInfo[0];

        private Text _text;
        protected void Awake()
        {
            _text = GetComponent<Text>();
        }


        public void AnimateText(string msg)
        {
            _text.text = "";
            switch (animationType)
            {
                case AnimationType.LetterStep:
                    StartCoroutine(_SetTextLetterStep(msg));
                    break;
                case AnimationType.WordStep:
                    StartCoroutine(_SetTextWordStep(msg));
                    break;
                case AnimationType.FadeIn:
                    StartCoroutine(_SetTextFadeIn(msg));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator _SetTextLetterStep(string msg)
        {
            var waitTime = new WaitForSeconds(1f/animationSpeed);

            int index = 0;
            while (index < msg.Length)
            {
                _text.text += msg[index];
                PlayRandomStepClip();
                yield return waitTime;
                index++;
            }
        }

        private IEnumerator _SetTextWordStep(string msg)
        {
            var waitTime = new WaitForSeconds(1f/animationSpeed);

            var words = msg.Split(' ');

            int index = 0;
            while (index < words.Length)
            {
                _text.text += words[index] + " ";
                PlayRandomStepClip();
                yield return waitTime;
                index++;
            }
        }


        private IEnumerator _SetTextFadeIn(string msg)
        {
            _text.text = msg;

            var startColor = _text.color;
            startColor.a = 0f;
            _text.color = startColor;

            float time = 1f / animationSpeed;
            float timer = 0f;
            while (timer < time)
            {
                timer += Time.deltaTime;
                startColor.a += animationSpeed * Time.deltaTime;
                _text.color = startColor;

                PlayRandomStepClip();
                yield return null;
            }
        }

        protected void PlayRandomStepClip()
        {
            if (stepAudioClips.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, stepAudioClips.Length);
                AudioManager.AudioPlayOneShot(stepAudioClips[index]);
            }
        }
    }
}
