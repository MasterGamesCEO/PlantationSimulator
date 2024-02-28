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
            allPlatforms[i].SavePlatformData(slotIndex, i);
            Debug.Log("Saving each platform Data in platformDataHandler");
        }

        PlayerPrefs.Save();
    }

    public void LoadPlatformData(int slotIndex)
    {
        for (int i = 0; i < allPlatforms.Count; i++)
        {
            allPlatforms[i].LoadPlatformData(slotIndex, i);
        }
    }

    #endregion

    #region Unlock and Reset Platform Data

    public void ResetGameData(int slotIndex)
    {
        for (int i = 0; i < allPlatforms.Count; i++)
        {
            allPlatforms.Remove(allPlatforms[i]);
        }
    }

    #endregion

    #region Add to Platform

    public void AddToArray(PlatformData platform)
    {
        LoadPlatformData(_saveData.SlotLastSelectedData);
        if (!allPlatforms.Contains(platform))
        {
            allPlatforms.Add(platform);
            Debug.Log("ADDING TO PLATFORM ARRAY");
        }
        
    }

    #endregion
}