using UnityEngine;
using System.Collections;
using System;
using Devdog.General;

namespace Devdog.QuestSystemPro
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Set Quest Progress/Set Quest Progress On Destroy")]
    public sealed class SetQuestProgressOnDestroy : MonoBehaviour
    {
        public QuestProgressDecorator progress;


        private bool _isQuitting;
        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private void OnDestroy()
        {
            if (_isQuitting)
            {
                return;
            }

            progress.Execute();
        }
    }
}