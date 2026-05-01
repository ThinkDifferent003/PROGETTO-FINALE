using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionManager
{
    public enum SpawnType { DEFAULT, PORTAL, SAVE_POINT}

    private const string MODE_KEY = "NextSpawnMode";
    private const string DETAIL_KEY = "NextSpawnDetail";
    public static void SetNextPosition(SpawnType type, string detail = "")
    {
        PlayerPrefs.SetInt(MODE_KEY, (int)type);
        PlayerPrefs.SetString(DETAIL_KEY, detail);
        PlayerPrefs.Save();
    }
    public static SpawnType GetCurrentType()
    {
        int typeIndex = PlayerPrefs.GetInt(MODE_KEY,0);
        return (SpawnType)typeIndex;
    }
    public static string GetDetail()
    {
        return PlayerPrefs.GetString(DETAIL_KEY, "");
    }
    public static void Clear()
    {
        PlayerPrefs.DeleteKey(MODE_KEY);
        PlayerPrefs.DeleteKey(DETAIL_KEY);
        PlayerPrefs.Save();
    }
}
