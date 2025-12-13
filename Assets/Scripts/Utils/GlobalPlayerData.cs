using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GlobalPlayerData
{
    public static string Username { get; set; }
    public static int HighScore { get; set; }

    public static void SetPlayerData(string user, int score)
    {
        Username = user;
        HighScore = score;
    }
}
