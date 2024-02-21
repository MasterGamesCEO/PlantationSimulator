using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    public float playerMoney;
    public PlotDataHandler plotDataList;
    private int _slotLastSelected;
    public bool _ifPlotReset0 = false;
    public bool _ifPlotReset1 = false;
    public bool _ifPlotReset2 = false;

    public int SlotLastSelectedData
    {
        get { return _slotLastSelected; }
        set { _slotLastSelected = value; }
    }
    private float _playerMoneySave;
    
    public float PlayerMoneySave
    {
        get { return _playerMoneySave; }
        set { _playerMoneySave = value; }
    }

    private static SaveData _instance;
    public static SaveData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveData>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("SaveData");
                    _instance = go.AddComponent<SaveData>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
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

    public bool DoesSaveExist(int slotIndex)
    {
        return PlayerPrefs.HasKey($"SaveSlot_{slotIndex}");
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public SaveData Reset(int slotIndex)
    {
        // Reset the data for the specified slotIndex
        ResetGameData(slotIndex);
        SaveUIState(slotIndex);
        SaveGameData(slotIndex);
    
        // Return the updated SaveData instance
        return this;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Load(int slotIndex)
    {
        Debug.Log($"Loading Save {slotIndex}");
        LoadGameData(slotIndex);
        LoadUIState(slotIndex);
    }

    public void SaveGameData(int slotIndex)
    {
        plotDataList = FindObjectOfType<PlotDataHandler>();
        if (plotDataList != null)
        {
            plotDataList.SavePlotData(slotIndex);
        }
        PlayerPrefs.SetFloat($"PlayerMoney_{slotIndex}", playerMoney);
        _playerMoneySave = playerMoney;
        
        Debug.Log($"Saving data for slot {slotIndex}, PlayerMoney: {playerMoney}");
        PlayerPrefs.Save();
    }

    public void LoadGameData(int slotIndex)
    {
        if (slotIndex == 0 && _ifPlotReset0 || slotIndex == 1 && _ifPlotReset1 || slotIndex == 2 && _ifPlotReset2)
        {
            ResetGameData(slotIndex);
            if (slotIndex == 0 && _ifPlotReset0)
            {
                _ifPlotReset0 = false;
            }
            else if (slotIndex == 1 && _ifPlotReset1)
            {
                _ifPlotReset1 = false;
            } else if (slotIndex == 2 && _ifPlotReset2)
            {
                _ifPlotReset2 = false;
            }
            
        }
        else
        {
            plotDataList = FindObjectOfType<PlotDataHandler>();
            if (plotDataList != null)
            {
                plotDataList.LoadPlotData(slotIndex);
            }
            PlayerController playerController = FindObjectOfType<PlayerController>();
            playerMoney = PlayerPrefs.GetFloat($"PlayerMoney_{slotIndex}", 10000f);
            _playerMoneySave = playerMoney;
            if (playerController != null)
            {
                playerController.SetPlayerMoney(playerMoney);
            }
        
            Debug.Log($"Loading data for slot {slotIndex}, PlayerMoney: {playerMoney}");
        }
        
    }

    private void ResetGameData(int slotIndex)
    {
        
        plotDataList = FindObjectOfType<PlotDataHandler>();
        
        
        PlayerPrefs.SetFloat($"PlayerMoney_{slotIndex}", 10000f);
        playerMoney = 10000f;
        _playerMoneySave = 10000f;
        
        plotDataList.ResetGameData(slotIndex);
        
        
    }

    private void SaveUIState(int slotIndex)
    {
        PlayerPrefs.SetInt($"SlotLastSelected_{slotIndex}", SlotLastSelectedData);
        PlayerPrefs.Save();
    }

    private void LoadUIState(int slotIndex)
    {
        SlotLastSelectedData = PlayerPrefs.GetInt($"SlotLastSelected_{slotIndex}", 0);
    }
}
