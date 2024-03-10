using System;
using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] public PlatformStats stats;
    [SerializeField] public Transform spawnPosition;
    [SerializeField] public GameObject currentRobotPrefab;
    [SerializeField] public RobotAttributes currentRobotStats;
    public PlatformDataHandler platformDataHandler;

    private void Start()
    {
        PlatformDataHandler dataHandler = FindObjectOfType<PlatformDataHandler>();
        if (!CurrentData.Instance.gameplayData.gameplayPlatformStats.Contains(stats))
        {
            dataHandler.LoadPlatformData(this);
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
        currentRobotPrefab =
            stats.robotType.Equals("basicRobot") ? FindObjectOfType<RobotManager>().basicPrefab :
            stats.robotType.Equals("silverRobot") ? FindObjectOfType<RobotManager>().silverPrefab :
            stats.robotType.Equals("goldRobot") ? FindObjectOfType<RobotManager>().goldPrefab :
            stats.robotType.Equals("diamondRobot") ? FindObjectOfType<RobotManager>().diamondPrefab :
            stats.robotType.Equals("ultraRobot") ? FindObjectOfType<RobotManager>().ultraPrefab: null;
        if (currentRobotPrefab != null)
        {
            Debug.Log(currentRobotPrefab.name);
            Instantiate(currentRobotPrefab, spawnPosition);
            Debug.Log("Added Robot To Scene");
            stats.hasRobotPrefab = true;
        }
    }
}
