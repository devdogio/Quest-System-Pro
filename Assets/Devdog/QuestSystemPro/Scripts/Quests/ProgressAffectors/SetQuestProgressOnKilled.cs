using UnityEngine;
using System.Collections;
using System;
using Devdog.General;

namespace Devdog.QuestSystemPro
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Set Quest Progress/Set Quest Progress On Kill")]
    public sealed class SetQuestProgressOnKilled : MonoBehaviour
    {
        public QuestProgressDecorator progress;

        public void OnKilled()
        {
            progress.Execute();
        }
    }
}