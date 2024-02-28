using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    public float playerMoney;
    private int _slotLastSelected;
    
    public PlotDataHandler plotDataList;
    public PlatformDataHandler platformDataList;
   
    [FormerlySerializedAs("_ifPlotReset0")] public bool ifPlotReset0 = false;
    [FormerlySerializedAs("_ifPlotReset1")] public bool ifPlotReset1 = false;
    [FormerlySerializedAs("_ifPlotReset2")] public bool ifPlotReset2 = false;

    public int SlotLastSelectedData
    {
        get { return _slotLastSelected; }
        set { _slotLastSelected = value; }
    }
    
    
    

    private static SaveData _instance;
    public static SaveData Instance
    {
        get
        {
            if (_instance == null)
            {

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
        return true;
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

    // ReSharper disable Unity.PerformanceAnalysis
    public void SaveGameData(int slotIndex)
    {
        plotDataList = FindObjectOfType<PlotDataHandler>();
        if (plotDataList != null)
        {
            plotDataList.SavePlotData(slotIndex);
        }
        platformDataList = FindObjectOfType<PlatformDataHandler>();
        if (platformDataList != null)
        {
            platformDataList.SavePlatformData(slotIndex);
            Debug.Log("Save platform data In saveData");
        }
        PlayerPrefs.SetFloat($"PlayerMoney_{slotIndex}", playerMoney);
        
        
        Debug.Log($"Saving data for slot {slotIndex}, PlayerMoney: {playerMoney}");
        PlayerPrefs.Save();
    }

    public void LoadGameData(int slotIndex)
    {
        if (slotIndex == 0 && ifPlotReset0 || slotIndex == 1 && ifPlotReset1 || slotIndex == 2 && ifPlotReset2)
        {
            Reset(slotIndex);
            if (slotIndex == 0 && ifPlotReset0)
            {
                ifPlotReset0 = false;
            }
            else if (slotIndex == 1 && ifPlotReset1)
            {
                ifPlotReset1 = false;
            } else if (slotIndex == 2 && ifPlotReset2)
            {
                ifPlotReset2 = false;
            }
            
        }
        else
        {
            plotDataList = FindObjectOfType<PlotDataHandler>();
            if (plotDataList != null)
            {
                plotDataList.LoadPlotData(slotIndex);
            }
            platformDataList = FindObjectOfType<PlatformDataHandler>();
            if (platformDataList != null)
            {
                platformDataList.LoadPlatformData(slotIndex);
            }
            PlayerController playerController = FindObjectOfType<PlayerController>();
            playerMoney = PlayerPrefs.GetFloat($"PlayerMoney_{slotIndex}", 10000f);
            
            PlotPricePopupScript plotPricePopupScript = FindObjectOfType<PlotPricePopupScript>();
            plotPricePopupScript.UpdateMoney(0);
            if (playerController != null)
            {
                playerController.SetPlayerMoney(playerMoney);
            }
            Debug.Log($"Loading money for slot {slotIndex}, PlayerMoney: {playerMoney}");
            Debug.Log($"Loading data for slot {slotIndex}, PlayerMoney: {playerMoney}");
        }
        
    }

    private void ResetGameData(int slotIndex)
    {
        plotDataList = FindObjectOfType<PlotDataHandler>();
        platformDataList = FindObjectOfType<PlatformDataHandler>();
        
        PlayerPrefs.SetFloat($"PlayerMoney_{slotIndex}", 10000f);
        playerMoney = 10000f;
        
        plotDataList.ResetGameData(slotIndex);
        platformDataList.ResetGameData(slotIndex);
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.SetPlayerMoney(playerMoney);
        PlotPricePopupScript plotPricePopupScript = FindObjectOfType<PlotPricePopupScript>();
        plotPricePopupScript.UpdateMoney(0);
        
        Debug.Log($"Resetting money for slot {slotIndex}, PlayerMoney: {playerMoney}");
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
