using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public enum ColorType
    {
        RED,
        GREEN,
        BLUE
    }

    [CreateAssetMenu(menuName = "Obstacle Object")]
    public class ObstacleScriptableObject : ScriptableObject
    {
        public ColorType ObstacleColor;
    }
}


