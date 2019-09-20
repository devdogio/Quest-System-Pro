using System;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    [Serializable]
    public class CameraPositionLookup
    {
        public string from;
        public string to;
        public float duration;
        public bool interpolate
        {
            get { return string.IsNullOrEmpty(to) == false; }
        }

        public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }
}
