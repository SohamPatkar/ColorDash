using System.Collections;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.Networking;
using UnityEngine;
using System.Net.Security;

public class FirebaseInitializer : MonoBehaviour
{
    #region  private members

    private FirebaseFirestore db;
    private CollectionReference users;
    private Dictionary<string, object> scoreUpdate = new Dictionary<string, object>();
    private static FirebaseInitializer instance;
    private static string supabaseUrl = "https://localhost:7046/api";

    #endregion

    public static FirebaseInitializer Instance { get { return instance; } }

    void Awake()
    {
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                users = db.Collection("Users");
            }
            else
            {
                Debug.LogError("Firebase dependencies not resolved: " + task.Result);
            }
        });
    }

    public CollectionReference GetUsersCollection()
    {
        return users;
    }

    public void SetHighScore()
    {
        scoreUpdate["Score"] = GlobalPlayerData.HighScore;

#if UNITY_EDITOR
        StartCoroutine(SendUserToSupabase("/player"));
#endif

        users.Document(GlobalPlayerData.Username).UpdateAsync(scoreUpdate);
    }


    private IEnumerator SendUserToSupabase(string controller)
    {
        UserDto newUser = new UserDto
        {
            Name = GlobalPlayerData.Username,
            PasswordHash = GlobalPlayerData.PasswordHash,
            PasswordSalt = GlobalPlayerData.PasswordSalt,
            Score = GlobalPlayerData.HighScore
        };

        Debug.Log("Sending user data: " + newUser.Name + " " + newUser.PasswordHash + " " + newUser.PasswordSalt + " " + newUser.Score);

        string jsonData = JsonUtility.ToJson(newUser);

        using (UnityWebRequest sendUser = new UnityWebRequest(supabaseUrl + controller, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            sendUser.uploadHandler = new UploadHandlerRaw(bodyRaw);
            sendUser.downloadHandler = new DownloadHandlerBuffer();
            sendUser.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending to: " + sendUser.url);

            yield return sendUser.SendWebRequest();

            if (sendUser.result == UnityWebRequest.Result.Success)
                Debug.Log("Success: " + sendUser.downloadHandler.text);
            else
                Debug.LogError("Error: " + sendUser.error);
        }
    }
}
