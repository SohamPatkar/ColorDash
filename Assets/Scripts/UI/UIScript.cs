using System.Collections;
using System.Collections.Generic;
using System;
using Obstacle;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIScript : MonoBehaviour
{
    public event Action<ColorType> OnButtonPress;
    public event Action<int> OnGetHighScore;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI highScore;

    void Start()
    {
        usernameText.text = GlobalPlayerData.Username;
        GameService.Instance.GetPlayer().OnScoreChange += SetScoreText;
        GameService.Instance.OnPlayerDeath += ShowGameOverPanel;
    }

    public void ButtonPressBlue()
    {
        OnButtonPress?.Invoke(ColorType.BLUE);
    }

    public void ButtonPressGreen()
    {
        OnButtonPress?.Invoke(ColorType.GREEN);
    }

    public void ButtonPressRed()
    {
        OnButtonPress?.Invoke(ColorType.RED);
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    private void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetHighScoreText(int score)
    {
        highScore.text = score.ToString();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }

    void OnDisable()
    {
        GameService.Instance.GetPlayer().OnScoreChange -= SetScoreText;
        GameService.Instance.OnPlayerDeath -= ShowGameOverPanel;
    }
}
