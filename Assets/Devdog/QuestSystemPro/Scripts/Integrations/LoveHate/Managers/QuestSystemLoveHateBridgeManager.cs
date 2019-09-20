#if LOVE_HATE

using System.Collections.Generic;
using Devdog.General;
using PixelCrushers.LoveHate;
using UnityEngine;

namespace Devdog.QuestSystemPro.Integration.LoveHate
{
    public class QuestSystemLoveHateBridgeManager : MonoBehaviour
    {
        private static FactionManager _factionManager;
        public static FactionManager factionManager
        {
            get
            {
                if (_factionManager == null)
                {
                    _factionManager = FindObjectOfType<FactionManager>();
                }

                return _factionManager;
            }
            protected set { _factionManager = value; }
        }
    }
}

#endif