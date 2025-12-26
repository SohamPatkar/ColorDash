using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    #region  private members

    private FirebaseFirestore db;
    private CollectionReference users;
    private Dictionary<string, object> scoreUpdate = new Dictionary<string, object>();
    private static FirebaseInitializer instance;

    #endregion

    public static FirebaseInitializer Instance { get { return instance; } }

    void Awake()
    {
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

        users.Document(GlobalPlayerData.Username).UpdateAsync(scoreUpdate);
    }
}
