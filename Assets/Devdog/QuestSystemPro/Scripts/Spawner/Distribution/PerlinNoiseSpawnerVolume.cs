using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    public class PerlinNoiseSpawnerVolume : WeightedTextureSpawnerVolume
    {
        [Range(0.2f, 30f)]
        public float noiseScale = 0.05f;

        [Range(0f, 1f)]
        public float cutoff;

        [Range(0f, 10f)]
        public float contrast = 1f;

        public override Texture2D distributionTexture
        {
            get
            {
                if (texture == null)
                {
                    texture = GeneratePerlinNoiseTexture(TextureGenerationWidth, TextureGenerationHeight, noiseScale, cutoff, contrast);
                }

                return texture;
            }
            set { texture = value; }
        }

        public const int TextureGenerationWidth = 32;
        public const int TextureGenerationHeight = 32;

        public static Texture2D GeneratePerlinNoiseTexture(int width, int height, float noiseScale, float cutoff, float contrast)
        {
            var colors = new Color[width * height];
            uint counter = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var c = Mathf.PerlinNoise(i / (float)width * noiseScale, j / (float)height * noiseScale);
                    c -= cutoff;
                    c *= contrast;
                    c = Mathf.Clamp01(c);

                    colors[counter++] = new Color(c, 0f, 0f, c);
                }
            }

            var tex = new Texture2D(width, height, TextureFormat.RGBA32, true, true);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.SetPixels(colors);
            tex.Apply(false);

            return tex;
        }
    }
}
