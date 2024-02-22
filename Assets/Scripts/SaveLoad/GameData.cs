using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private int _selectedSlot;

    public int SelectedGameSlot
    {
        get { return _selectedSlot; }
        set { _selectedSlot = value; }
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