using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveLoadSystem
{
    
    public static void SaveData(SaveData data, int slotIndex)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString($"SaveSlot_{slotIndex}", json);
        PlayerPrefs.Save();
    }

    public static SaveData LoadData(int slotIndex)
    {
        string json = PlayerPrefs.GetString($"SaveSlot_{slotIndex}");
        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

        if (loadedData == null)
        {
            Debug.LogWarning($"Failed to load data for slot {slotIndex}. Creating a new SaveData instance.");
            loadedData = new SaveData();
        }

        return loadedData;
    }

    public static bool DoesSaveExist(int slotIndex)
    {
        return PlayerPrefs.HasKey($"SaveSlot_{slotIndex}");
    }

    public static SaveData ResetData(int slotIndex)
    {
        SaveData newData = LoadData(slotIndex);
        newData.Reset();
        SaveData(newData, slotIndex);
        return newData;
    }

    public static void CreateNewSave(int slotIndex)
    {
        SaveData newData = new SaveData();
        newData.playerMoney = 10000f;
        SaveData(newData, slotIndex);
    }

    public static void SaveGameData(PlayerController playerController, PlotDataHandler plotDataHandler)
    {
        SaveData saveData = new SaveData();
        saveData.playerMoney = playerController.GetPlayerMoney();

        if (plotDataHandler != null)
        {
            saveData.plotDataList = plotDataHandler.GetPlotDataList();
            plotDataHandler.SavePlotData();
        }

        SaveData(saveData, saveData.SlotLastSelectedData);
        Debug.Log($"Saving player money: {saveData.playerMoney}");
    }

    public static void LoadGameData(PlayerController playerController, PlotDataHandler plotDataHandler)
    {
        SaveData saveData =  new SaveData();
        saveData = LoadData(saveData.SlotLastSelectedData);

        if (saveData != null)
        {
            if (plotDataHandler != null)
            {
                plotDataHandler.LoadPlotData(saveData.plotDataList);
            }

            playerController.SetPlayerMoney(saveData.playerMoney);
        }
        Debug.Log($"Loaded player money: {saveData.playerMoney}");
    }
}
