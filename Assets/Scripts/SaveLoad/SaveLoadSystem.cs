using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveLoadSystem
{
    public static void SaveData(SaveData data, int slotIndex)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveSlot_" + slotIndex, json);
        PlayerPrefs.Save();
    }

    public static SaveData LoadData(int slotIndex)
    {
        string json = PlayerPrefs.GetString("SaveSlot_" + slotIndex);
        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

        if (loadedData == null)
        {
            Debug.LogWarning($"Failed to load data for slot {slotIndex}. Creating a new SaveData instance.");
            loadedData = new SaveData();
        }

        Debug.Log($"Loaded player money: {loadedData.playerMoney}");
    
        return loadedData;
    }


    public static bool DoesSaveExist(int slotIndex)
    {
        return PlayerPrefs.HasKey("SaveSlot_" + slotIndex);
    }

    public static void DeleteSave(int slotIndex)
    {
        PlayerPrefs.DeleteKey("SaveSlot_" + slotIndex);
        PlayerPrefs.Save();
    }

    public static void SaveGameData(PlayerController playerController, PlotDataHandler plotDataHandler)
    {
        SaveData saveData = new SaveData();
        saveData.playerMoney = playerController.GetPlayerMoney();

        if (plotDataHandler != null)
        {
            saveData.plotDataList = plotDataHandler.GetPlotDataList();
        }

        SaveData(saveData, 0); // Assuming you're using slotIndex 0
        Debug.Log($"Saving player money: {saveData.playerMoney}");
    }

    public static void LoadGameData(PlayerController playerController, PlotDataHandler plotDataHandler)
    {
        Debug.Log("LoadGameData method called");
        
        SaveData saveData = LoadData(0);
        

        if (saveData != null)
        {
            if (plotDataHandler != null)
            {
                plotDataHandler.LoadPlotData(saveData.plotDataList);
            }

            // Set player money after loading plot data
            playerController.SetPlayerMoney(saveData.playerMoney);
            Debug.Log("LoadGameData"+ saveData.playerMoney); 
        }
        Debug.Log($"Loaded player money: {saveData.playerMoney}");
    }
}