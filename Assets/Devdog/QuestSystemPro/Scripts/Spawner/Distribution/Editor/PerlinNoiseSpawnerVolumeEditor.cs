using Devdog.General;
using UnityEditor;
using UnityEngine;

namespace Devdog.QuestSystemPro.Editors
{
    [CustomEditor(typeof(PerlinNoiseSpawnerVolume))]
    public class PerlinNoiseSpawnerVolumeEditor : WeightedTextureSpawnerVolumeEditor
    {
        private Vector2 _volumeSize;
        private float _noiseScale;
        private float _constrast;
        private float _cutoff;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var volume = (PerlinNoiseSpawnerVolume) target;
            if (volume.volumeSize != _volumeSize ||
                Mathf.Approximately(volume.noiseScale, _noiseScale) == false ||
                Mathf.Approximately(volume.cutoff, _cutoff) == false ||
                Mathf.Approximately(volume.contrast, _constrast) == false)
            {
                _volumeSize = volume.volumeSize;
                _noiseScale = volume.noiseScale;
                _constrast = volume.contrast;
                _cutoff = volume.cutoff;

                if (EditorUtility.IsPersistent(volume.distributionTexture) == false)
                {
                    volume.distributionTexture = PerlinNoiseSpawnerVolume.GeneratePerlinNoiseTexture(PerlinNoiseSpawnerVolume.TextureGenerationWidth, PerlinNoiseSpawnerVolume.TextureGenerationHeight, volume.noiseScale, volume.cutoff, volume.contrast);
                }
            }
        }
    }
}
