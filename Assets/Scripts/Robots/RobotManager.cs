using System;
using System.Collections.Generic;
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
    
    [SerializeField] public List<RobotInfo> workingRobots;
    [SerializeField] public List<RobotInfo> unassignedRobots;

    private SaveData _saveData;

    private void Start()
    {
        _saveData = SaveData.Instance;
    }

    [System.Serializable]
    public class RobotInfo
    {
        public GameObject robotPrefab;
        public RobotAttributes attributes; // Add information about the robot
    }
    

    public void AssignRobotToAPlatform(Platform platform, int selectedAvailableRobotIndex)
    {

        if (unassignedRobots[selectedAvailableRobotIndex] != null)
        {
            RobotInfo selectedRobotInfo = unassignedRobots[selectedAvailableRobotIndex];
            GameObject newRobot = Instantiate(selectedRobotInfo.robotPrefab, platform.spawnPosition.transform.position, Quaternion.identity);

            // Remove the selected robot from available list
            unassignedRobots.Remove(selectedRobotInfo);

            // Add the assigned robot to the list
            workingRobots.Add(new RobotInfo()
            {
                robotPrefab = newRobot,
                attributes = selectedRobotInfo.attributes
            });
            
            platform.stats.isAssigned = true;
            platform.currentRobotPrefab = selectedRobotInfo.robotPrefab;
            platform.currentRobotStats = selectedRobotInfo.attributes;
            platform.platformDataHandler.SavePlatformData();
        }
        else
        {
            Debug.Log("No robot in that slot");
        }
    }

    public void BuyRobot(RobotAttributes attributes)
    {
        if (unassignedRobots.Count < 4)
        {
            RobotInfo newRobot = new RobotInfo
            {
                robotPrefab = 
                    attributes.robotType.Equals("basicRobot") ? basicPrefab :
                    attributes.robotType.Equals("silverRobot") ? silverPrefab :
                    attributes.robotType.Equals("goldRobot") ? goldPrefab :
                    attributes.robotType.Equals("diamondRobot") ? diamondPrefab :
                    attributes.robotType.Equals("ultraRobot") ? ultraPrefab : null,
                attributes = attributes
            };
            unassignedRobots.Add(newRobot);
        }
        else
        {
            Debug.Log("Maximum number of available robots reached");
        }
        
    }
    
}
