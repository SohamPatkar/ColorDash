using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class BlueObstacle : ObstacleController
    {
        public BlueObstacle(ColorType colorType, ObstacleView obstacleView, GameObject spawnPoint) : base(colorType, obstacleView, spawnPoint)
        {
            this.obstacleView.SetController(this);
            this.obstacleView.SetColor(colorType);
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
            GameService.Instance.obstaclePool.ReturnEnemyToPool(this);
        }
    }
}


