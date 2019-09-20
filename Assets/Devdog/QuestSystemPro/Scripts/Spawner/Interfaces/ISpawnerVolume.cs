using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Devdog.QuestSystemPro
{
    /// <summary>
    /// An abstraction of a volume for the spawner. 
    /// Specific distributions such as guassian or voronoi can be added in the implementations.
    /// </summary>
    public interface ISpawnerVolume
    {
        /// <summary>
        /// 
        /// <remarks>
        ///     Returned location should be in local space (relative to the spawner).
        /// </remarks>
        /// 
        /// </summary>
        /// <returns>A vector3 location inside the represented volume.</returns>
        Vector3 GetPointInVolume(SpawnerBase spawner, SpawnerCategoryInfo category);
    }
}
