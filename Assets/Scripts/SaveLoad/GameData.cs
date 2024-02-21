using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private int selectedSlot;

    public int SelectedGameSlot
    {
        get { return selectedSlot; }
        set { selectedSlot = value; }
    }
    // Singleton pattern
    private static GameData _instance;
    public static GameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameData>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameData");
                    _instance = go.AddComponent<GameData>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
}