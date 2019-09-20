using System;
using Devdog.General;
using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Devdog.QuestSystemPro
{
    public class WeightedTextureSpawnerVolume : MonoBehaviour, ISpawnerVolume
    {
        public Vector2 volumeSize = new Vector2(40f, 40f);

        public enum Channel
        {
            R = 0,
            G = 1,
            B = 2,
            A = 3
        }

        public Channel channel;

        [SerializeField]
        [Required]
        protected Texture2D texture;
        /// <summary>
        /// The texture used to create grab points. Values of 0 have 0% change of being in the final result, values of 1 have the highest chance
        /// </summary>
        public virtual Texture2D distributionTexture
        {
            get { return texture; }
            set { texture = value; }
        }

        public SpatialTreeCell rootCell;

        protected virtual void Awake()
        {
#if UNITY_5_4_OR_NEWER
            Random.InitState(System.DateTime.Now.Millisecond);
#endif

            if (distributionTexture != null)
            {
                rootCell = GenerateRootCellForTexture(distributionTexture);
            }
        }

        public SpatialTreeCell GenerateRootCellForTexture(Texture2D texture)
        {
            var v = texture.GetPixels();
            float[] values;
            switch (channel)
            {
                case Channel.R:
                    values = v.Select(o => o.r).ToArray();
                    break;
                case Channel.G:
                    values = v.Select(o => o.g).ToArray();
                    break;
                case Channel.B:
                    values = v.Select(o => o.b).ToArray();
                    break;
                case Channel.A:
                    values = v.Select(o => o.a).ToArray();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new SpatialTreeCell(texture.width, texture.height, 0, 0, values, null);
        }

        public Vector3 GetPointInVolume(SpawnerBase spawner, SpawnerCategoryInfo category)
        {
            Assert.IsNotNull(distributionTexture, "Distribution texture is null, this isn't allowed!");

            Vector2 outValue = new Vector2();
            GetRandomValueFromSpatialTreeRecursive(rootCell, distributionTexture.width, distributionTexture.height, ref outValue);

            // Normalize outValue
            outValue.x /= rootCell.width;
            outValue.y /= rootCell.height;

            outValue.x *= volumeSize.x;
            outValue.y *= volumeSize.y;

            outValue.x -= volumeSize.x/2f;
            outValue.y -= volumeSize.y/2f;

            return new Vector3(outValue.x, 0f, outValue.y);
        }

        private void GetRandomValueFromSpatialTreeRecursive(SpatialTreeCell spatialTreeCell, int width, int height, ref Vector2 outValue)
        {
            Assert.IsNotNull(spatialTreeCell);

            if (spatialTreeCell.element00 != null)
            {
                Assert.IsNotNull(spatialTreeCell.element00);
                Assert.IsNotNull(spatialTreeCell.element01);
                Assert.IsNotNull(spatialTreeCell.element10);
                Assert.IsNotNull(spatialTreeCell.element11);

                var r = Random.Range(0f, spatialTreeCell.sumValue);
                if (spatialTreeCell.element00.sumValue >= r)
                {
                    GetRandomValueFromSpatialTreeRecursive(spatialTreeCell.element00, width >> 1, height >> 1, ref outValue);
                    return;
                }

                r -= spatialTreeCell.element00.sumValue;
                if (spatialTreeCell.element01.sumValue >= r)
                {
                    GetRandomValueFromSpatialTreeRecursive(spatialTreeCell.element01, width >> 1, height >> 1, ref outValue);
                    return;
                }

                r -= spatialTreeCell.element01.sumValue;
                if (spatialTreeCell.element10.sumValue >= r)
                {
                    GetRandomValueFromSpatialTreeRecursive(spatialTreeCell.element10, width >> 1, height >> 1, ref outValue);
                    return;
                }

                r -= spatialTreeCell.element10.sumValue;
                if (spatialTreeCell.element11.sumValue >= r)
                {
                    GetRandomValueFromSpatialTreeRecursive(spatialTreeCell.element11, width >> 1, height >> 1, ref outValue);
                    return;
                }

                DevdogLogger.LogVerbose("This should never be called.... " + r + " left..");
            }
            else
            {
                // Grab (weighted) from the remaining cells.
                var sum = spatialTreeCell.sumValue;
                var r = Random.Range(0, sum);
                int counter = 0;
                int x = 0;
                int y = 0;
                bool shouldBreak = false;
                for (x = spatialTreeCell.absoluteX; x < spatialTreeCell.absoluteX + spatialTreeCell.width; x++)
                {
                    if (shouldBreak)
                    {
                        break;
                    }

                    for (y = spatialTreeCell.absoluteY; y < spatialTreeCell.absoluteY + spatialTreeCell.height; y++)
                    {
                        if (spatialTreeCell.values[counter] >= r)
                        {
                            shouldBreak = true;
                            break;
                        }

                        r -= spatialTreeCell.values[counter];
                        counter++;
                    }
                }

                // At the end, no more child cells
                outValue.x = y;
                outValue.y = x;
            }
        }
    }
}
