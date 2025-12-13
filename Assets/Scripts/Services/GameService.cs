using System;
using System.Collections;
using System.Collections.Generic;
using Obstacle;
using Player;
using UnityEngine;

public class GameService : MonoBehaviour
{
    [SerializeField] private ObstacleView obstacleView;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private UIScript uIScript;
    [SerializeField] private PlayerScript player;
    [SerializeField] private List<ColorType> colorTypes;
    [SerializeField] private float obstacleSpeed = 0;
    [SerializeField] private float speedMultiplier = 0;

    private static GameService instance;
    public static GameService Instance { get { return instance; } }
    public ObstaclePool obstaclePool;
    public UIScript UIScript { get { return uIScript; } }
    public event Action OnPlayerDeath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UIScript.SetHighScoreText(GetHighScore());
        obstaclePool = new ObstaclePool();
        SpawnObstacle();
    }

    public void SpawnObstacle()
    {
        if (player.GetPlayerState() == PlayerState.Alive)
        {
            ColorType colorType = GetRandomColor();
            ObstacleController obstacle = obstaclePool.GetObstacle(colorType, obstacleView);
            obstacle.Activate();
        }
        else
        {
            SaveHighScore(player.GetPlayerScore());
            OnPlayerDeath.Invoke();
        }
    }

    public void SaveHighScore(int score)
    {
        Debug.Log("Saving High Score: " + GlobalPlayerData.HighScore);

        if (score > GlobalPlayerData.HighScore)
        {
            GlobalPlayerData.HighScore = score;
            FirebaseInitializer.Instance.SetHighScore();
        }
    }

    private int GetHighScore()
    {
        return GlobalPlayerData.HighScore;
    }

    public void ChangeObstacleSpeed()
    {
        obstacleSpeed += Time.deltaTime * speedMultiplier;
    }

    public float GetObstacleSpeed()
    {
        return obstacleSpeed;
    }

    private ColorType GetRandomColor()
    {
        return colorTypes[UnityEngine.Random.Range(0, colorTypes.Count)];
    }

    public PlayerScript GetPlayer()
    {
        return player;
    }

    public ObstacleController CreateObstacle(ColorType colorType, ObstacleView obstacleView)
    {
        switch (colorType)
        {
            case ColorType.RED:
                return new RedObstacle(colorType, obstacleView, spawnPoint);

            case ColorType.GREEN:
                return new GreenObstacle(colorType, obstacleView, spawnPoint);

            case ColorType.BLUE:
                return new BlueObstacle(colorType, obstacleView, spawnPoint);

            default:
                return new ObstacleController(colorType, obstacleView, spawnPoint);
        }
    }
}
