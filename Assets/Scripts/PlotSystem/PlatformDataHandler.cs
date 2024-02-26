using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDataHandler : MonoBehaviour
{
    [SerializeField] public PlatformData[] allPlatforms;
    private SaveData _saveData;

    #region Save and Load Platform Data

    private void Start()
    {
        _saveData = SaveData.Instance;
    }

    public void SavePlatformData(int slotIndex)
    {
        for (int i = 0; i < allPlatforms.Length; i++)
        {
            PlayerPrefs.SetInt($"Platform_{slotIndex}_{i}_IsAssigned", allPlatforms[i].isAssigned ? 1 : 0);
            // Add additional data saving as needed
        }

        PlayerPrefs.Save();
    }

    public void LoadPlatformData(int slotIndex)
    {
        for (int i = 0; i < allPlatforms.Length; i++)
        {
            int isAssignedValue = PlayerPrefs.GetInt($"Platform_{slotIndex}_{i}_IsAssigned", 0);
            allPlatforms[i].isAssigned = isAssignedValue == 1;
            // Add additional data loading as needed

            allPlatforms[i].SetRobotPrefab(allPlatforms[i].isAssigned, allPlatforms[i].assignedRobotPrefab);
        }
    }

    #endregion

    #region Unlock and Reset Platform Data

    public void ResetGameData(int slotIndex)
    {
        for (int i = 0; i < allPlatforms.Length; i++)
        {
            PlayerPrefs.DeleteKey($"Platform_{slotIndex}_{i}_IsAssigned");
            allPlatforms[i].isAssigned = false;
            // Add additional reset logic as needed

            allPlatforms[i].SetRobotPrefab(allPlatforms[i].isAssigned, allPlatforms[i].noRobotPrefab);
        }
    }

    #endregion

    #region Add to Platform

    public void AddToPlatform(Component robotPrefab)
    {
        foreach (var platform in allPlatforms)
        {
            if (!platform.isAssigned)
            {
                platform.SetRobotPrefab(true, robotPrefab);
                platform.isAssigned = true;
                SavePlatformData(_saveData.SlotLastSelectedData);
                return; // Exit the loop after assigning the robot to the first available platform
            }
        }
        
        Debug.LogError("No available platform to assign the robot.");
    }

    #endregion
}