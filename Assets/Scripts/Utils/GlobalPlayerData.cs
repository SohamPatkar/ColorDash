using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GlobalPlayerData
{
    public static string Username;
    public static string PasswordHash;
    public static string PasswordSalt;
    public static int HighScore;

    public static void SetPlayerData(string user, int score, string passwordSalt, string passwordHash)
    {
        Username = user;
        HighScore = score;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
    }
}
