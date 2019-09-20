using System;
using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro
{
    /// <summary>
    /// A cell that's used to make weighted lookup cheaper.
    /// It works by generating LOD's of the existing data using cells, this way the performance will be roughly O(N)
    /// </summary>
    public class SpatialTreeCell
    {
        public int width;
        public int height;

        public int absoluteX { get; private set; }
        public int absoluteY { get; private set; }

        public float[] values;

        // The 4 grid elements
        public SpatialTreeCell element00;
        public SpatialTreeCell element01;
        public SpatialTreeCell element10;
        public SpatialTreeCell element11;

        public float averageValue { get; private set; }
        public float maxValue { get; private set; }
        public float sumValue { get; private set; }

        public SpatialTreeCell parent { get; private set; }

        public SpatialTreeCell(int width, int height, int absoluteX, int absoluteY, float[] values, SpatialTreeCell parent)
        {
            this.width = width;
            this.height = height;
            this.absoluteX = absoluteX;
            this.absoluteY = absoluteY;
            this.parent = parent;
            this.values = values;

            if (width > 4 && height > 4)
            {
                element00 = HandleBlock(0, 0);
                element01 = HandleBlock(0, 1);
                element10 = HandleBlock(1, 0);
                element11 = HandleBlock(1, 1);

                // Grab from the child values, these will recursively be set (most efficient)
                averageValue = (element00.averageValue + element01.averageValue + element10.averageValue + element11.averageValue) / 4f;
                sumValue = (element00.sumValue + element01.sumValue + element10.sumValue + element11.sumValue);
                maxValue = Mathf.Max(element00.maxValue, element01.maxValue, element10.maxValue, element11.maxValue);
            }
            else
            {
                // There's no more children, calculate from our values.
                this.averageValue = this.values.Average();
                this.maxValue = this.values.Max();
                this.sumValue = this.values.Sum();
            }
        }

        private SpatialTreeCell HandleBlock(int x, int y)
        {
            int halfWidth = width >> 1;
            int halfHeight = height >> 1;

            // Grab 1/4th of the current block
            // [ X - ]
            // [ - - ]
            int indexOffsetX = halfWidth * x * width;
            int indexOffsetY = halfHeight * y;

            var newValues = new float[halfWidth * halfHeight];
            int counter = 0;
            for (int i = indexOffsetX; i < indexOffsetX + halfWidth; i++)
            {
                for (int j = indexOffsetY; j < indexOffsetY + halfHeight; j++)
                {
                    int sourceIndex = (width * (i - indexOffsetX)) + indexOffsetX + j;
                    newValues[counter] = values[sourceIndex];
                    counter++;
                }
            }

            return new SpatialTreeCell(halfWidth, halfHeight, absoluteX + (halfWidth * x), absoluteY + (halfHeight * y), newValues, this);
        }
    }
}
