#if LIPSYNC

using System;
using Devdog.General;
using RogoDigital.Lipsync;

namespace Devdog.QuestSystemPro.Dialogue
{
    public partial class NodeBase
    {
        [ForceStandardObjectPicker]
        public LipSyncData lipsyncData;
    }
}

#endif