using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

public class FirebaseLoginManager : MonoBehaviour
{
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;

    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonPressed);

        registerButton.onClick.AddListener(OnRegisterButtonPressed);
    }

    private void OnRegisterButtonPressed()
    {
        string hash, salt;
        PasswordHasher.CreatePasswordHash(password.text, out hash, out salt);

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
                Debug.LogError("Register FAILED: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("User registered successfully");
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

        if (string.IsNullOrEmpty(username.text) || string.IsNullOrEmpty(password.text))
        {
            Debug.LogError("Username or password is empty");
            return;
        }

        string enteredUsername = username.text;
        string enteredPassword = password.text;

        FirebaseInitializer.Instance.GetUsersCollection().Document(enteredUsername)
        .GetSnapshotAsync()
        .ContinueWithOnMainThread(task =>
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
                return;
            }

            string playerUsername = snapshot.GetValue<string>("UserName");
            int playerScore = snapshot.GetValue<int>("Score");

            GlobalPlayerData.SetPlayerData(playerUsername, playerScore);

            string storedHash = snapshot.GetValue<string>("PasswordHash");
            string storedSalt = snapshot.GetValue<string>("PasswordSalt");

            bool isValid = PasswordHasher.VerifyPassword(enteredPassword, storedHash, storedSalt);

            if (isValid)
            {
                Debug.Log("LOGIN SUCCESS");

                long score = snapshot.GetValue<long>("Score");
                Debug.Log("User score: " + score);

                SceneManager.LoadScene("MainGame");
            }
            else
            {
                Debug.Log("INVALID PASSWORD");
            }
        });
    }
}
