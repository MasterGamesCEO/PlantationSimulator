using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformData : MonoBehaviour
{
    [SerializeField] public bool isAssigned = false;
    [SerializeField] public BoxCollider spawnPosition;
    [SerializeField] public GameObject currentRobotPrefab;
    [SerializeField] public RobotAttributes currentRobotStats;

    private void Start()
    {
        PlatformDataHandler platformDataHandler = FindObjectOfType<PlatformDataHandler>();
        platformDataHandler.addToArray(this);
    }

    public void savePlatformData()
    {
        // Save robot type PlayerPrefs.SetInt($"RobotPrefab", currentRobotPrefab);
        // Save Robot Attributes
        
    }
    public void loadPlatformData()
    {
        // Save robot type PlayerPrefs.SetInt($"RobotPrefab", currentRobotPrefab);
        // Save Robot Attributes
        
    }
    public void resetPlatformData()
    {
        //TODO: make sure the array removes the robots instance
        // Save robot type PlayerPrefs.SetInt($"RobotPrefab", currentRobotPrefab);
        // Save Robot Attributes
        
    }

    public bool IsAssigned
    {
        get => isAssigned;
        set => isAssigned = value;
    }
}
