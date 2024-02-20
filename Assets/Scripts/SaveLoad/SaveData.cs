using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float playerMoney;
    public List<PlotData> plotDataList = new List<PlotData>();
    public HomeScreen HomeScreen = new HomeScreen();
    
    public int SlotLastSelectedData
    {
        get { return HomeScreen.SlotLastSelected; }
        set { HomeScreen.SlotLastSelected = value; }
    }
    [System.Serializable]
    public class PlotData
    {
        public bool isLocked;

        public PlotData(bool isLocked)
        {
            this.isLocked = isLocked;
        }
    }

    

    public void Load(int slotIndex)
    {
        LoadGameData();
        LoadUIState(slotIndex);
    }

    public void Reset()
    {
        ResetGameData();
        SaveGameData();
    }

    #region Game Data

    private void SaveGameData()
    {
        PlayerPrefs.SetFloat("PlayerMoney", playerMoney);

        for (int i = 0; i < plotDataList.Count; i++)
        {
            PlayerPrefs.SetInt($"Plot_{i}_IsLocked", plotDataList[i].isLocked ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    private void LoadGameData()
    {
        playerMoney = PlayerPrefs.GetFloat("PlayerMoney", 10000f);

        for (int i = 0; i < plotDataList.Count; i++)
        {
            int isLockedValue = PlayerPrefs.GetInt($"Plot_{i}_IsLocked", 1);
            plotDataList[i].isLocked = isLockedValue == 1;
        }
    }

    private void ResetGameData()
    {
        playerMoney = 10000f;

        for (int i = 0; i < plotDataList.Count; i++)
        {
            plotDataList[i].isLocked = true;
        }

        if (plotDataList.Count > 0)
        {
            plotDataList[0].isLocked = false;
        }
    }

    #endregion

    #region UI State

    private void SaveUIState(int slotIndex)
    {
        PlayerPrefs.SetString($"ActivePanel_Scene_{slotIndex}", "MainMenu");
    }

    private void LoadUIState(int slotIndex)
    {
        string activePanel = PlayerPrefs.GetString($"ActivePanel_Scene_{slotIndex}", "MainMenu");
    }

    #endregion
}