#if LIPSYNC__

using System;
using RogoDigital.Lipsync;

namespace Devdog.General
{
    public partial class LocalizedAudioClipInfo
    {
        [ForceStandardObjectPicker]
        public LipSyncData lipsyncData;
    }
}

#endif