using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Unity.Burst.Intrinsics;

[FirestoreData]
public class User
{
    [FirestoreProperty]
    public string UserName { get; set; }

    [FirestoreProperty]
    public string PasswordHash { get; set; }

    [FirestoreProperty]
    public string PasswordSalt { get; set; }

    [FirestoreProperty]
    public long Score { get; set; }
}

[Serializable]
public class UserDto
{
    public string Name;
    public string PasswordHash;
    public string PasswordSalt;
    public int Score;
}

public class FirebaseLoginManager : MonoBehaviour
{
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;
    [SerializeField] Button playButton;
    [SerializeField] Button leaderBoardSceneButton;
    [SerializeField] TextMeshProUGUI feedbackText;
    [SerializeField] GameObject loginForm;

    #region  private variables

    private bool isUsernameValid;
    private bool isPasswordValid;
    private string hash;
    private string salt;
    private string enteredUsername;
    private string enteredPassword;
    private string playerUsername;
    private string playerPasswordHash;
    private string playerPasswordSalt;
    private string storedHash;
    private string storedSalt;
    private int playerScore;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (GlobalPlayerData.Username != null)
        {
            loginForm.SetActive(false);
            playButton.gameObject.SetActive(true);
            playButton.onClick.AddListener(PlayButtonClicked);
        }
        else
        {
            playButton.gameObject.SetActive(false);
            loginForm.SetActive(true);
        }

        loginButton.onClick.AddListener(OnLoginButtonPressed);

        leaderBoardSceneButton.onClick.AddListener(LoadLeaderboardScene);

        registerButton.onClick.AddListener(OnRegisterButtonPressed);
    }

    private void PlayButtonClicked()
    {
        SceneManager.LoadScene("MainGame");
    }

    private void LoadLeaderboardScene()
    {
        SceneManager.LoadScene("LeaderBoardScene");
    }

    private bool ValidateInputs()
    {
        return !string.IsNullOrEmpty(username.text) && !string.IsNullOrEmpty(password.text);
    }

    private void OnRegisterButtonPressed()
    {
        PasswordHasher.CreatePasswordHash(password.text, out hash, out salt);

        if (ValidateInputs() == false)
        {
            feedbackText.text = "Please enter username and password.";
            return;
        }

        User newUser = new User
        {
            UserName = username.text,
            PasswordHash = hash,
            PasswordSalt = salt,
            Score = 0
        };

        FirebaseInitializer.Instance.GetUsersCollection().Document(username.text)
        .SetAsync(newUser)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                feedbackText.text = "Registration failed.";
            }
            else if (task.IsCompleted)
            {
                feedbackText.text = "Registered Successfully.";
                registerButton.enabled = false;
                registerButton.image.color = Color.gray;
            }
        });
    }

    private void OnLoginButtonPressed()
    {
        if (FirebaseInitializer.Instance.GetUsersCollection() == null)
        {
            Debug.LogError("Firebase not ready");
            return;
        }

        if (ValidateInputs() == false)
        {
            Debug.LogError("Username or password is empty");
            feedbackText.text = "Please enter username and password.";
            return;
        }

        enteredUsername = username.text.Trim();
        enteredPassword = password.text;

        DocumentReference user = FirebaseInitializer.Instance.GetUsersCollection().Document(enteredUsername);

        user.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Login failed: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            if (!snapshot.Exists)
            {
                Debug.Log("User not found");
                feedbackText.text = "User not found.";
                return;
            }

            playerUsername = snapshot.GetValue<string>("UserName");
            playerPasswordHash = snapshot.GetValue<string>("PasswordHash");
            playerPasswordSalt = snapshot.GetValue<string>("PasswordSalt");
            playerScore = snapshot.GetValue<int>("Score");

            GlobalPlayerData.SetPlayerData(playerUsername, playerScore, playerPasswordSalt, playerPasswordHash);

            storedHash = snapshot.GetValue<string>("PasswordHash");
            storedSalt = snapshot.GetValue<string>("PasswordSalt");

            bool isValid = PasswordHasher.VerifyPassword(enteredPassword, storedHash, storedSalt);

            if (isValid)
            {
                long score = snapshot.GetValue<long>("Score");

                SceneManager.LoadScene("MainGame");
            }
            else
            {
                feedbackText.text = "Invalid password.";
            }
        });
    }
}
