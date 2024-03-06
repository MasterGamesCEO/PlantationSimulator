using System;
using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformDataHandler : MonoBehaviour
{
    [SerializeField] public List<Platform> allPlatforms;
    private SaveData _saveData;

    #region Save and Load Platform Data

    private void Start()
    {
        _saveData = SaveData.Instance;
    }

    #region Save and Load Plot Data

    public void SavePlatformData()
    {
        List<PlatformStats> savePlatformStats = new List<PlatformStats>();
        for (int i = 0; i < allPlatforms.Count; i++)
        {
            savePlatformStats.Add(allPlatforms[i].GetPlatformStats());
        }

        CurrentData.Instance.gameplayData.GameplayPlatformStats = savePlatformStats;
    }

    public void LoadPlatformData()
    {
        
        for (int i = 0; i < allPlatforms.Count; i++)
        {
            allPlatforms[i].SetPlatformStats(CurrentData.Instance.gameplayData.GameplayPlatformStats[i]);
            if (allPlatforms[i].stats.isAssigned)
            {
                allPlatforms[i].addRobotToScene();
            }
        }
        
    }

    #endregion
    
    
    public List<PlotStats> ResetPlatformData()
    {
        List<PlatformStats> savePlatformStats = new List<PlatformStats>();
        for (int i = 0; i < 3; i++)
        {
            allPlatforms[0].stats.isLocked = false;
            allPlots[0].DeactivateBoundary();
        }
        for (int i = 1; i < allPlots.Count; i++)
        {
            allPlots[i].stats.isLocked= true;
            allPlots[i].ActivateBoundary();
            allPlots[i].SetPlotColor(allPlots[i].stats.isLocked);
            savePlotStats.Add(allPlots[i].GetPlotStats());
        }
        
        return savePlotStats;
    }

    #endregion

    #region Add to Platform

    public void AddToArray(Platform platform)
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