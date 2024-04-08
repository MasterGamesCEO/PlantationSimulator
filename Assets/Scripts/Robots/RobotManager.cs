using System;
using System.Collections.Generic;
using SaveLoad;
using UnityEngine;
[System.Serializable]
public class RobotManager : MonoBehaviour
{
    // Robot Prefabs
    [Header("Auction prefab")]
    [SerializeField] public GameObject basicScrawnyACPrefab;
    [SerializeField] public GameObject basicNormalACPrefab;
    [SerializeField] public GameObject silverNormalACPrefab;
    [SerializeField] public GameObject silverTallACPrefab;
    [SerializeField] public GameObject goldTallACPrefab;
    [SerializeField] public GameObject goldBuiltACPrefab;
    [SerializeField] public GameObject diamondBuiltACPrefab;
    [SerializeField] public GameObject diamondMassiveACPrefab;
    [SerializeField] public GameObject ultraMassiveACPrefab;
    [SerializeField] public GameObject ultraTankACPrefab;
    [Header("Plot prefab")]
    [SerializeField] public GameObject basicScrawnyPrefab;
    [SerializeField] public GameObject basicNormalPrefab;
    [SerializeField] public GameObject silverNormalPrefab;
    [SerializeField] public GameObject silverTallPrefab;
    [SerializeField] public GameObject goldTallPrefab;
    [SerializeField] public GameObject goldBuiltPrefab;
    [SerializeField] public GameObject diamondBuiltPrefab;
    [SerializeField] public GameObject diamondMassivePrefab;
    [SerializeField] public GameObject ultraMassivePrefab;
    [SerializeField] public GameObject ultraTankPrefab;
    
    [SerializeField] public List<RobotAttributes> workingRobots;
    [SerializeField] public List<RobotAttributes> unassignedRobots;
    [SerializeField] public List<RobotAttributes> auctionRobots;

    private SaveData _saveData;

    public void SaveRobotData()
    {
        List<RobotAttributes> saveWorkingRobotAttributesList = new List<RobotAttributes>();
        List<RobotAttributes> saveUnassignedRobotAttributesList = new List<RobotAttributes>();
        for (int i = 0; i < workingRobots.Count; i++)
        {
            saveWorkingRobotAttributesList.Add(workingRobots[i]);
            Debug.Log("CPS For " + i + " " + workingRobots[i].cps);
        }
        for (int i = 0; i < unassignedRobots.Count; i++)
        {
            saveUnassignedRobotAttributesList.Add(unassignedRobots[i]);
            Debug.Log("CPS For " + i + " " + unassignedRobots[i].cps);
        }
        CurrentData.Instance.robotData.workingRobots = saveWorkingRobotAttributesList;
        CurrentData.Instance.robotData.unassignedRobots = saveUnassignedRobotAttributesList;
    }

    public void LoadRobotData()
    {
        workingRobots = CurrentData.Instance.robotData.workingRobots;
        unassignedRobots = CurrentData.Instance.robotData.unassignedRobots;
    }
    private void Start()
    {
        _saveData = SaveData.Instance;
    }
    
    public void BuyRobot(RobotAttributes attributes)
    {
        if (unassignedRobots.Count < 4)
        {
            unassignedRobots.Add(attributes);
        }
        else
        {
            Debug.Log("Maximum number of available robots reached");
        }
        
    }
    
}
