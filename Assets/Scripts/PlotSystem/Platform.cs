using System;
using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] public PlatformStats stats;
    [SerializeField] public BoxCollider spawnPosition;
    [SerializeField] public GameObject currentRobotPrefab;
    [SerializeField] public RobotAttributes currentRobotStats;
    public PlatformDataHandler platformDataHandler;

    private void Start()
    {
        PlatformDataHandler dataHandler = FindObjectOfType<PlatformDataHandler>();
        if (!CurrentData.Instance.gameplayData.gameplayPlatformStats.Contains(stats))
        {
            dataHandler.AddToArray(this);
        }
        else
        {
            Debug.Log("Contains "+ stats);
        }
    }
    

    
    public void SetPlatformStats(PlatformStats loadedStats)
    {
        stats = loadedStats;
    }

    public PlatformStats GetPlatformStats()
    {
        return stats;
    }
    

    public bool IsAssigned
    {
        get => stats.isAssigned;
        set => stats.isAssigned = value;
    }

    public void AddRobotToScene()
    {
        if (!stats.hasRobotPrefab)
        {
            if (currentRobotPrefab != null)
            {
                
            }
            Debug.Log("Added Robot To Scene");
            stats.hasRobotPrefab = true;
        }
    }
}
