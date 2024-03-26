using System.Collections;
using System.Collections.Generic;
using SaveLoad;
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
    public RobotManager robotManager;
   
    public bool ifPlotReset0 = false;
    public bool ifPlotReset1 = false;
    public bool ifPlotReset2 = false;

    public int SlotLastSelectedData
    {
        get => _slotLastSelected;
        set => _slotLastSelected = value;
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

    public bool DoesSaveExist(int slotIndex)
    {
        return true;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Reset(int slotIndex)
    {
        CurrentData.Instance.Reset();
        SaveGameData(slotIndex);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Load(int slotIndex)
    {
        Debug.Log($"Loading Save {slotIndex}");
        LoadGameData(slotIndex);
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SaveGameData(int slotIndex)
    {
        plotDataList = FindObjectOfType<PlotDataHandler>();
        if (plotDataList != null)
        {
            plotDataList.SavePlotData();
        }
        platformDataList = FindObjectOfType<PlatformDataHandler>();
        if (platformDataList != null)
        {
            platformDataList.SavePlatformData();
        }
        robotManager = FindObjectOfType<RobotManager>();
        if (robotManager != null)
        {
            robotManager.SaveRobotData();
        }
        CurrentData.Instance.SaveFile();
        Debug.Log($"Saving data for slot {slotIndex}");
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
            CurrentData.Instance.LoadFile();
            plotDataList = FindObjectOfType<PlotDataHandler>();
            if (plotDataList != null)
            {
                plotDataList.LoadPlotData();
            }
            robotManager = FindObjectOfType<RobotManager>();
            if (robotManager != null)
            {
                robotManager.LoadRobotData();
            }
            PlayerController playerController = FindObjectOfType<PlayerController>();
            playerMoney = CurrentData.Instance.uiData.saveMoney;
            playerController.SetPlayerMoney(playerMoney);
            NumberCounter numberCounter = FindObjectOfType<NumberCounter>();
            numberCounter.Value = CurrentData.Instance.uiData.cropData;
            
            PlotPricePopupScript plotPricePopupScript = FindObjectOfType<PlotPricePopupScript>();
            plotPricePopupScript.UpdateMoney(0);
            if (playerController != null)
            {
                playerController.SetPlayerMoney(playerMoney);
            }
            Debug.Log($"Loading money for slot {slotIndex}, PlayerMoney: {playerMoney}");
        }
        
    }
    
    
}
