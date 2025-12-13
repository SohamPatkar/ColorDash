using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Obstacle
{
    public class ObstacleView : MonoBehaviour
    {
        private ObstacleController obstacleController;
        private SpriteRenderer colorOfSprite;

        void Awake()
        {
            colorOfSprite = gameObject.GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            obstacleController.ObstacleMovement(GameService.Instance.GetObstacleSpeed());
        }

        public void SetColor(ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType.RED:
                    colorOfSprite.color = Color.red;
                    break;

                case ColorType.GREEN:
                    colorOfSprite.color = Color.green;
                    break;

                case ColorType.BLUE:
                    colorOfSprite.color = Color.blue;
                    break;

                default:
                    colorOfSprite.color = Color.red;
                    break;
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            if (player && (colorOfSprite.color == player.GetSpriteRenderColor()))
            {
                player.OnHit();
                obstacleController.ObstacleOnHit();
                SoundManager.Instance.PlaySfxSound(SoundType.Collected);
                obstacleController.ReturnToPool();
            }
            else
            {
                player.SetPlayerState(PlayerState.Dead);
                player.OnHit();
                SoundManager.Instance.PlaySfxSound(SoundType.Death);
                obstacleController.ObstacleOnHit();
                gameObject.SetActive(false);
            }
        }

        public void SetController(ObstacleController obstacleController)
        {
            this.obstacleController = obstacleController;
        }
    }
}


