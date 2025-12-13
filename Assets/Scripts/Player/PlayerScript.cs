using Obstacle;
using System;
using UnityEngine;

namespace Player
{
    public enum PlayerState
    {
        Alive,
        Dead
    }

    public class PlayerScript : MonoBehaviour
    {
        #region  private members

        private int playerScore;
        private PlayerState playerState;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        #endregion

        [SerializeField] private ParticleSystem particleSystem;

        public event Action<int> OnScoreChange;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            GameService.Instance.UIScript.OnButtonPress += SetColor;
            playerState = PlayerState.Alive;
        }

        public int GetPlayerScore()
        {
            return playerScore;
        }

        public void AddScore()
        {
            playerScore += 1;
            OnScoreChange?.Invoke(playerScore);
        }

        public void OnHit()
        {
            animator.SetTrigger("Hit");
            PlayParticleSysetem();

            if (playerState == PlayerState.Alive)
            {
                AddScore();
            }
        }

        public void PlayParticleSysetem()
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Clear();
            }

            particleSystem.Play();
        }

        public void SetPlayerState(PlayerState state)
        {
            playerState = state;
        }

        public PlayerState GetPlayerState()
        {
            return playerState;
        }

        public Color GetSpriteRenderColor()
        {
            return spriteRenderer.color;
        }

        private void SetColor(ColorType colorType)
        {
            var main = particleSystem.main;

            switch (colorType)
            {
                case ColorType.RED:
                    spriteRenderer.color = Color.red;
                    main.startColor = Color.red;
                    break;

                case ColorType.GREEN:
                    spriteRenderer.color = Color.green;
                    main.startColor = Color.green;
                    break;

                case ColorType.BLUE:
                    spriteRenderer.color = Color.blue;
                    main.startColor = Color.blue;
                    break;

                default:
                    spriteRenderer.color = Color.red;
                    main.startColor = Color.red;
                    break;
            }

        }

        void OnDisable()
        {
            GameService.Instance.UIScript.OnButtonPress -= SetColor;
        }
    }
}


