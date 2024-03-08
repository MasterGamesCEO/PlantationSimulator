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

        CurrentData.Instance.gameplayData.gameplayPlatformStats = savePlatformStats;
    }

    public void LoadPlatformData()
    {
        Debug.Log("Loadplatformdata");
        for (int i = 0; i < CurrentData.Instance.gameplayData.gameplayPlatformStats.Count; i++)
        {
            if (CurrentData.Instance.gameplayData.gameplayPlatformStats[i] != null)
            {
                foreach (var t in allPlatforms)
                {
                    allPlatforms[i].SetPlatformStats(CurrentData.Instance.gameplayData.gameplayPlatformStats[i]);
                    if (allPlatforms[i].stats.isAssigned && !allPlatforms[i].stats.hasRobotPrefab) 
                    { 
                        allPlatforms[i].AddRobotToScene();
                    }
                    Debug.Log("Platform"+allPlatforms[i].name+" stats loaded");
                }
                
            }
            
            
        }
        
    }

    #endregion
    
    
    public List<PlatformStats> ResetPlatformData()
    {
        List<PlatformStats> savePlatformStats = new List<PlatformStats>();
        
        
        
        return savePlatformStats;
    }

    #endregion

    #region Add to Platform

    public void AddToArray(Platform platform)
    {
        List<PlatformStats> savePlatformStats = new List<PlatformStats>();
        allPlatforms.Add(platform);
       // platform.CreatePlatformStats;
        savePlatformStats.Add(platform.GetPlatformStats());
        CurrentData.Instance.gameplayData.gameplayPlatformStats = savePlatformStats;
    }

    #endregion
}