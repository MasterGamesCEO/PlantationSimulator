using System;
using System.Collections.Generic;
using SaveLoad;
using UnityEngine;
[System.Serializable]
public class RobotManager : MonoBehaviour
{
    // Robot Prefabs
    [SerializeField] public GameObject basicPrefab;
    [SerializeField] public GameObject silverPrefab;
    [SerializeField] public GameObject goldPrefab;
    [SerializeField] public GameObject diamondPrefab;
    [SerializeField] public GameObject ultraPrefab;
    
    [SerializeField] public List<RobotAttributes> workingRobots;
    [SerializeField] public List<RobotAttributes> unassignedRobots;

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
