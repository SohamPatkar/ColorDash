using UnityEngine;
using Firebase.Firestore;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardScript : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject leaderboardPrefab;
    [SerializeField] private Button homeButton;

    // Start is called before the first frame update
    void Start()
    {
        if (homeButton != null)
        {
            homeButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Login");
            });
        }

        PopulateLeaderboard();
    }

    private async void PopulateLeaderboard()
    {
        if (leaderboardPrefab == null)
        {
            return;
        }

        var userCollection = FirebaseInitializer.Instance.GetUsersCollection();

        var users = userCollection.OrderByDescending("Score").Limit(10);

        QuerySnapshot snapshot = await users.GetSnapshotAsync();

        foreach (Transform child in leaderboardPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            if (!document.Exists)
                continue;

            User user = document.ConvertTo<User>();

            GameObject entry = Instantiate(leaderboardPrefab, leaderboardPanel.transform);
            TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = user.UserName;
            texts[1].text = user.Score.ToString();
        }
    }
}
