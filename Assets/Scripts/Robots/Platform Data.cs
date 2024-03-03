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
    public PlatformDataHandler platformDataHandler;

    private void Start()
    {
        PlatformDataHandler platformDataHandler = FindObjectOfType<PlatformDataHandler>();
        platformDataHandler.AddToArray(this);
    }

    public void SavePlatformData(int slotIndex, int platformIndex)
    {
        PlayerPrefs.SetInt($"Platform_{slotIndex}_{platformIndex}_IsAssigned", isAssigned ? 1 : 0);

        if (isAssigned)
        {
            // Save robot prefab name
            PlayerPrefs.SetString($"Platform_{slotIndex}_{platformIndex}_RobotPrefab", currentRobotPrefab.name);

            // Save robot attributes
            currentRobotStats.SaveAttributes($"Platform_{slotIndex}_{platformIndex}");
        }
        Debug.Log("Saved platform data for"+ slotIndex + " " + platformIndex);
    }
    public void LoadPlatformData(int slotIndex, int platformIndex)
    {
        int isAssignedValue = PlayerPrefs.GetInt($"Platform_{slotIndex}_{platformIndex}_IsAssigned", 0);
        isAssigned = isAssignedValue == 1;

        if (isAssigned)
        {
            // Load robot prefab
            string robotPrefabName = PlayerPrefs.GetString($"Platform_{slotIndex}_{platformIndex}_RobotPrefab", "");
            currentRobotPrefab = Resources.Load<GameObject>("Robots/" + robotPrefabName);

            // Load robot attributes
            currentRobotStats.LoadAttributes($"Platform_{slotIndex}_{platformIndex}");
        }
        Debug.Log("Loaded platform data for"+ slotIndex + " " + platformIndex);
    }
    public void ResetPlatformData()
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
