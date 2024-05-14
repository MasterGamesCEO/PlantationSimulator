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
            stats.robotType.Equals("basic") && stats.size.Equals("Scrawny") ? FindObjectOfType<RobotManager>().basicScrawnyPrefab :
            stats.robotType.Equals("basic") && stats.size.Equals("Normal") ? FindObjectOfType<RobotManager>().basicNormalPrefab :
            stats.robotType.Equals("silver") && stats.size.Equals("Normal") ? FindObjectOfType<RobotManager>().silverNormalPrefab :
            stats.robotType.Equals("silver") && stats.size.Equals("Tall") ? FindObjectOfType<RobotManager>().silverTallPrefab :
            stats.robotType.Equals("gold") && stats.size.Equals("Tall") ? FindObjectOfType<RobotManager>().goldTallPrefab :
            stats.robotType.Equals("gold") && stats.size.Equals("Built") ? FindObjectOfType<RobotManager>().goldBuiltPrefab :
            stats.robotType.Equals("diamond") && stats.size.Equals("Built") ? FindObjectOfType<RobotManager>().diamondBuiltPrefab :
            stats.robotType.Equals("diamond") && stats.size.Equals("Massive") ? FindObjectOfType<RobotManager>().diamondMassivePrefab :
            stats.robotType.Equals("ultra") && stats.size.Equals("Massive") ? FindObjectOfType<RobotManager>().ultraMassivePrefab :
            stats.robotType.Equals("ultra") && stats.size.Equals("Tank") ? FindObjectOfType<RobotManager>().ultraTankPrefab :
            null;
        if (currentRobotPrefab != null)
        {
            Debug.Log(currentRobotPrefab.name);
            Instantiate(currentRobotPrefab, spawnPosition);
            Animator robotAnimator = currentRobotPrefab.GetComponent<Animator>();
            if ( robotAnimator != null)
            {
                robotAnimator.SetBool("IsFarming", true);
                
            }
            Debug.Log("Added Robot To Scene");
            stats.hasRobotPrefab = true;
        }
        else
        {
            Debug.Log("Null");
        }
    }

    

    public void RemoveRobotFromScene()
    {
        Debug.Log("trying to remove");
        if (currentRobotPrefab != null)
        {
            Debug.Log(currentRobotPrefab.name);
            
            Transform objTransform = transform.Find("Spawn Position");
            Transform obj = objTransform.Find(currentRobotPrefab.name + "(Clone)");
            if (obj != null)
            {
                Destroy(obj.gameObject);
                Debug.Log("Removed Robot To Scene");
                stats.hasRobotPrefab = false;
            }
        }
    }
}