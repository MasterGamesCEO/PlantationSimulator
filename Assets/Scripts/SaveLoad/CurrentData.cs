using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class CurrentData : MonoBehaviour
{
    
    [SerializeField] public float SaveMoney
    {
        get => uiData.saveMoney;
        set => uiData.saveMoney = value;
    }
    [SerializeField] public List<PlotStats> PlotStats
    {
        get => gameplayData.gameplayPlotStats;
        set => gameplayData.gameplayPlotStats = value;
    }
    
    public UiData uiData = new UiData();
    public GameplayData gameplayData = new GameplayData();

    [System.Serializable]
    public class UiData
    {
        public float saveMoney;
    }
    [System.Serializable]
    public class GameplayData
    {
        public List<PlotStats> gameplayPlotStats;
    }
   

    public void SaveFile()
    {
        if (FindObjectOfType<PlotDataHandler>())
        {
            SavePlotsToJson();
        }

        if (FindObjectOfType<PlotPricePopupScript>())
        {
            SaveMoneyToJson();
        }
    }
    public void LoadFile()
    {
        if (FindObjectOfType<PlotDataHandler>())
        {
            LoadPlotsFromJson();
        }

        if (FindObjectOfType<PlotPricePopupScript>())
        {
            LoadMoneyFromJson();
        }
    }
    
    public void SaveMoneyToJson()
    {
        string filePath = Application.persistentDataPath + $"/SavedMoneyDataFor{_slotLastSelected}.json";
        Debug.Log(filePath);
        
        string currentData = JsonUtility.ToJson(uiData);
        Debug.Log(currentData);
        System.IO.File.WriteAllText(filePath,currentData);
        
        Debug.Log("Saved Money");
    }
    public void SavePlotsToJson()
    {
        string filePath = Application.persistentDataPath + $"/SavedPlotDataFor{_slotLastSelected}.json";
        Debug.Log(filePath);
        
        string currentData = JsonUtility.ToJson(gameplayData);
        Debug.Log(currentData);
        System.IO.File.WriteAllText(filePath,currentData);
        
        Debug.Log("Saved Plots");
    }
    public void LoadMoneyFromJson()
    {
        string filePath = Application.persistentDataPath + $"/SavedMoneyDataFor{_slotLastSelected}.json";
        string currentData = System.IO.File.ReadAllText(filePath);
        Debug.Log(currentData);
        uiData = JsonUtility.FromJson<UiData>(currentData);
        SaveMoney = uiData.saveMoney;
        Debug.Log("Loaded Money");
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void LoadPlotsFromJson()
    {
        string filePath = Application.persistentDataPath + $"/SavedPlotDataFor{_slotLastSelected}.json";

        if (System.IO.File.Exists(filePath))
        {
            string currentData = System.IO.File.ReadAllText(filePath);
            Debug.Log(currentData);

            try
            {
                gameplayData = JsonUtility.FromJson<GameplayData>(currentData);

                if (gameplayData != null)
                {
                    if (gameplayData.gameplayPlotStats != null)
                    {
                        PlotStats = new List<PlotStats>(gameplayData.gameplayPlotStats);
                        
                        Debug.Log($"Loaded Plots: {PlotStats.Count} plots");
                    }
                    else
                    {
                        Debug.LogWarning("Loaded Plot data's 'gameplayPlotStats' is null.");
                    }
                }
                else
                {
                    Debug.LogWarning("Deserialization of GameplayData failed.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error deserializing JSON: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"File not found at path: {filePath}");
        }
    }


    public void Reset()
    {
        PlotDataHandler plotDataHandler = FindObjectOfType<PlotDataHandler>();
        UiData newUiData = new UiData
        {
            saveMoney = 10000f
        };
        GameplayData newGameplayData = new GameplayData()
        {
            gameplayPlotStats = plotDataHandler.ResetPlotData()
        };
        
        string currentMoneyData = JsonUtility.ToJson(newUiData);
        string currentPlotData = JsonUtility.ToJson(newGameplayData);
        string filePathMoney = Application.persistentDataPath + $"/SavedMoneyDataFor{_slotLastSelected}.json";
        string filePathPlots = Application.persistentDataPath + $"/SavedPlotDataFor{_slotLastSelected}.json";
        System.IO.File.WriteAllText(filePathMoney,currentMoneyData);
        System.IO.File.WriteAllText(filePathPlots,currentPlotData);
        Debug.Log("Reset json");
        LoadFile();
    }
    
    #region Singleton Data
    private int _slotLastSelected;
    public int SlotLastSelectedData
    {
        get => _slotLastSelected;
        set => _slotLastSelected = value;
    }
    
    private static CurrentData _instance;
    public static CurrentData Instance
    {
        get
        {
            if (_instance == null)
            {

                if (_instance == null)
                {
                    GameObject go = new GameObject("CurrentData");
                    _instance = go.AddComponent<CurrentData>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    #endregion
    
}