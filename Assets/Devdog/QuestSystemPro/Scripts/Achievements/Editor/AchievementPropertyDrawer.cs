using System;
using System.Collections.Generic;
using Devdog.QuestSystemPro.Editors;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    [CustomPropertyDrawer(typeof(Achievement))]
    public class AchievementPropertyDrawer : QuestPropertyDrawerBase<Achievement>
    {

    }
}
