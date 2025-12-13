using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;

namespace Obstacle
{
    public class ObstacleController
    {
        protected ColorType colorType;
        protected ObstacleView obstacleView;
        protected Transform spawnPoint;
        private float speed = 0.5f;

        public ObstacleController(ColorType colorType, ObstacleView obstacleView, GameObject spawnPoint)
        {
            this.spawnPoint = spawnPoint.transform;
            this.colorType = colorType;
            this.obstacleView = Object.Instantiate(obstacleView.gameObject, spawnPoint.transform.position, quaternion.identity).GetComponent<ObstacleView>();
        }

        public void Activate()
        {
            obstacleView.gameObject.SetActive(true);
            obstacleView.gameObject.transform.position = spawnPoint.position;
        }

        public virtual void ObstacleMovement(float speed)
        {
            obstacleView.transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * speed;
        }

        public ColorType GetObstacleColor()
        {
            return colorType;
        }

        public void ObstacleOnHit()
        {
            GameService.Instance.ChangeObstacleSpeed();
            GameService.Instance.SpawnObstacle();
            obstacleView.gameObject.SetActive(false);
        }

        public virtual void ReturnToPool() { }
    }
}


