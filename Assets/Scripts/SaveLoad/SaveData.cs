using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float playerMoney;
    // Add any other data you want to save/load

    [System.Serializable]
    public class PlotData
    {
        public bool isLocked;
        // Add any other plot-related data you want to save/load

        public PlotData(bool isLocked)
        {
            this.isLocked = isLocked;
        }
    }

    public List<PlotData> plotDataList = new List<PlotData>();

    // Add other methods for saving/loading specific data

    #region Save and Load Methods

    public void Save()
    {
        SaveGameData();
        SaveUIState();
    }

    public void Load()
    {
        LoadGameData();
        LoadUIState();
    }

    #endregion

    #region Game Data

    private void SaveGameData()
    {
        
        PlayerPrefs.SetFloat("PlayerMoney", playerMoney);

        // Save plot data
        for (int i = 0; i < plotDataList.Count; i++)
        {
            PlayerPrefs.SetInt($"Plot_{i}_IsLocked", plotDataList[i].isLocked ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    private void LoadGameData()
    {
        
        playerMoney = 10000f;

        // Load plot data
        for (int i = 0; i < plotDataList.Count; i++)
        {
            int isLockedValue = PlayerPrefs.GetInt($"Plot_{i}_IsLocked", 1);
            plotDataList[i].isLocked = isLockedValue == 1;
            // You may want to call a method like setPlotColor here if needed
        }
        Debug.Log("SavedataLoadGameData"+ playerMoney); 
    }

    #endregion

    #region UI State

    private void SaveUIState()
    {
        PlayerPrefs.SetString("ActivePanel_Scene", "MainMenu"); // Adjust this as needed
        // Save other UI state data if necessary
    }

    private void LoadUIState()
    {
        string activePanel = PlayerPrefs.GetString("ActivePanel_Scene", "MainMenu"); // Adjust this as needed
        // Load other UI state data if necessary
    }

    #endregion
}