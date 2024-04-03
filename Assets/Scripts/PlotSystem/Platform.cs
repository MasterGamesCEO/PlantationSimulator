using System;
using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using UnityEngine;
using Object = System.Object;

public class Platform : MonoBehaviour
{
    [SerializeField] public PlatformStats stats;
    [SerializeField] public Transform spawnPosition;
    [SerializeField] public GameObject currentRobotPrefab;
    [SerializeField] public RobotAttributes currentRobotStats;
    public PlatformDataHandler platformDataHandler;
    public bool isUpdating;
    public bool firstUpdate = true;

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


    private void Update()
    {
        if (stats.isAssigned)
        {
            if (!isUpdating)
            {
                StartCoroutine(UpdateCrops());
            }
            
        }
    }
    IEnumerator UpdateCrops()
    {
        
        isUpdating = true;
        PlotPricePopupScript plotScript = FindObjectOfType<PlotPricePopupScript>();
        NumberCounter numberCounter = FindObjectOfType<NumberCounter>();
        if (!firstUpdate)
        {
            //plotScript.UpdateCrops(-stats.cps);
            CurrentData.Instance.uiData.cropData += stats.cps;
            numberCounter.Value = CurrentData.Instance.uiData.cropData;
        }
        else
        {
            firstUpdate = false;
        }
        yield return new WaitForSeconds(5);
        isUpdating = false;
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
    public void RemoveRobotFromScene()
    {
        Debug.Log("trying to remove");
        if (currentRobotPrefab != null)
        {
            Debug.Log(currentRobotPrefab.name);
            
            Transform objTransform = transform.Find("Spawn Position");
            Transform obj = objTransform.Find("Robot Blue(Clone)");
            if (obj != null)
            {
                Destroy(obj.gameObject);
                Debug.Log("Removed Robot To Scene");
                stats.hasRobotPrefab = false;
            }
        }
    }
}
