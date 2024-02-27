using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformDataHandler : MonoBehaviour
{
    [SerializeField] public List<PlatformData> allPlatforms;
    private SaveData _saveData;

    #region Save and Load Platform Data

    private void Start()
    {
        _saveData = SaveData.Instance;
    }

    public void SavePlatformData(int slotIndex)
    {
        for (int i = 0; i < allPlatforms.Count; i++)
        {
            allPlatforms[i].savePlatformData();
            PlayerPrefs.SetInt($"Platform_{slotIndex}_{i}_IsAssigned", allPlatforms[i].isAssigned ? 1 : 0);
            
        }

        PlayerPrefs.Save();
    }

    public void LoadPlatformData(int slotIndex)
    {
        for (int i = 0; i < allPlatforms.Count; i++)
        {
            int isAssignedValue = PlayerPrefs.GetInt($"Platform_{slotIndex}_{i}_IsAssigned", 0);
            allPlatforms[i].loadPlatformData();


        }
    }

    #endregion

    #region Unlock and Reset Platform Data

    public void ResetGameData(int slotIndex)
    {
        for (int i = 0; i < allPlatforms.Count; i++)
        {
            if (i < allPlatforms.Count)
            {
                PlayerPrefs.DeleteKey($"Platform_{slotIndex}_{i}_IsAssigned");
                allPlatforms[i].isAssigned = false;
            }

            //remove robot instance
        }
    }

    #endregion

    #region Add to Platform

    public void addToArray(PlatformData platform)
    {
        
        allPlatforms.Add(platform);
        SavePlatformData(_saveData.SlotLastSelectedData);
        Debug.Log("ADDING TO PLATFORM ARRAY");
    }

    #endregion
}