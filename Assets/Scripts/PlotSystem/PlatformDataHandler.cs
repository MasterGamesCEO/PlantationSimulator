using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Debug.Log("Save For " + i + " " + allPlatforms[i].stats.isAssigned);
        }

        CurrentData.Instance.gameplayData.gameplayPlatformStats = savePlatformStats;
    }

    public void LoadPlatformData(Platform platform)
    {
        Debug.Log("Loadplatformdata");
        /*
        for (int i = 0; i < CurrentData.Instance.gameplayData.gameplayPlatformStats.Count; i++)
        {
            if (CurrentData.Instance.gameplayData.gameplayPlatformStats[i] != null)
            {
                allPlatforms[i].SetPlatformStats(CurrentData.Instance.gameplayData.gameplayPlatformStats[i]);
                Debug.Log("Load For " + i + " " + CurrentData.Instance.gameplayData.gameplayPlatformStats[i].isAssigned);
                if (allPlatforms[i].stats.isAssigned && !allPlatforms[i].stats.hasRobotPrefab)
                { 
                    allPlatforms[i].AddRobotToScene();
                }
                Debug.Log("Platform"+allPlatforms[i].name+" stats loaded");
            }
            }
            */
        if (!allPlatforms.Contains(platform))
        {
            allPlatforms.Add(platform);
            int index = allPlatforms.IndexOf(platform);
            if ( index < CurrentData.Instance.gameplayData.gameplayPlatformStats.Count)
            {
                Debug.Log(allPlatforms[index].IsAssigned);
                Debug.Log(CurrentData.Instance.gameplayData.gameplayPlatformStats[index].isAssigned);
                allPlatforms[index].SetPlatformStats(CurrentData.Instance.gameplayData.gameplayPlatformStats[index]);
            }
            if (allPlatforms[index].stats.isAssigned)
            { 
                allPlatforms[index].AddRobotToScene();
            }
            Debug.Log("Platform"+allPlatforms[index].name+" stats loaded");
        }
        /*if (allPlatforms.Count <= CurrentData.Instance.gameplayData.gameplayPlatformStats.Count )
        {
            Debug.Log(allPlatforms.Count);
            Debug.Log(CurrentData.Instance.gameplayData.gameplayPlatformStats.Count );
            PlatformStats platformStats = CurrentData.Instance.gameplayData.gameplayPlatformStats.FindAll();
            Debug.Log(platformStats);
            allPlatforms[allPlatforms.Count].SetPlatformStats(platformStats);
            Debug.Log("Platform "+allPlatforms.Count+" stats loaded");
        }
        */

    }

    #endregion
    
    
    public List<PlatformStats> ResetPlatformData()
    {
        List<PlatformStats> savePlatformStats = new List<PlatformStats>();
        
        
        
        return savePlatformStats;
    }

    #endregion

    
}