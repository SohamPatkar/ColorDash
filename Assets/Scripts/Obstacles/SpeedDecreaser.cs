using System;
using UnityEngine;

namespace Obstacle
{
    public class SpeedDecreaser : ObstacleController
    {
        public SpeedDecreaser(ColorType colorType, ObstacleView obstacleView, GameObject spawnPoint) : base(colorType, obstacleView, spawnPoint)
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