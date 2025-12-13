using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class ObstaclePool
    {
        private List<PooledObstacle> obstacles = new List<PooledObstacle>();

        public ObstacleController GetObstacle(ColorType colorType, ObstacleView obstacleView)
        {
            if (obstacles.Count > 0)
            {
                PooledObstacle obstacleObject = obstacles.Find(obstacle => obstacle.Obstacle.GetObstacleColor() == colorType && !obstacle.IsUsed);

                if (obstacleObject != null)
                {
                    obstacleObject.IsUsed = true;
                    return obstacleObject.Obstacle;
                }
            }

            return CreateNewObstacle(colorType, obstacleView);
        }

        private ObstacleController CreateNewObstacle(ColorType colorType, ObstacleView obstacleView)
        {
            PooledObstacle obstacle = new PooledObstacle();
            obstacle.Obstacle = GameService.Instance.CreateObstacle(colorType, obstacleView);
            obstacle.IsUsed = true;
            obstacles.Add(obstacle);
            return obstacle.Obstacle;
        }

        public void ReturnEnemyToPool(ObstacleController returnObstacle)
        {
            PooledObstacle obstacle = obstacles.Find(item => item.Obstacle.Equals(returnObstacle));
            obstacle.IsUsed = false;
        }

        public class PooledObstacle
        {
            public ObstacleController Obstacle;
            public bool IsUsed;
        }
    }
}


