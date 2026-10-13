using UnityEngine;

public static class GameProgress
{
    public static void CompleteGame(int gameID)
    {
        PlayerPrefs.SetInt("GameCompleted_" + gameID, 1);
        PlayerPrefs.Save();
    }

    public static bool IsGameCompleted(int gameID)
    {
        return PlayerPrefs.GetInt("GameCompleted_" + gameID, 0) == 1;
    }

    public static void ResetProgress()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerPrefs.DeleteKey("GameCompleted_" + i);
        }

        PlayerPrefs.Save();
    }
}