using System;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    [Serializable]
    public class CameraPosition
    {
        public string key = "";
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public bool useWorldSpace = false;
    }
}
