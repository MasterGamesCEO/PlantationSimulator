using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [SerializeField] public List<AssignedRobotInfo> workingRobots;
    [SerializeField] public List<RobotInfo> unassignedRobots;

    [System.Serializable]
    public class RobotInfo
    {
        public GameObject robotPrefab;
        public RobotAttributes attributes; // Add information about the robot
    }

    [System.Serializable]
    public class AssignedRobotInfo
    {
        public GameObject robotPrefab;
        public PlatformData assignedPlatform;
        public RobotAttributes attributes;
    }

    public void AssignRobotToAPlatform(PlatformData platform, int selectedAvailableRobotIndex)
    {

        if (unassignedRobots[selectedAvailableRobotIndex] != null)
        {
            RobotInfo selectedRobotInfo = unassignedRobots[selectedAvailableRobotIndex];
            GameObject newRobot = Instantiate(selectedRobotInfo.robotPrefab, platform.spawnPosition.transform.position, Quaternion.identity);

            // Remove the selected robot from available list
            unassignedRobots.Remove(selectedRobotInfo);

            // Add the assigned robot to the list
            workingRobots.Add(new AssignedRobotInfo
            {
                robotPrefab = newRobot,
                assignedPlatform = platform,
                attributes = selectedRobotInfo.attributes
            });
            platform.isAssigned = true;
            platform.currentRobotPrefab = selectedRobotInfo.robotPrefab;
            platform.currentRobotStats = selectedRobotInfo.attributes;
        }
        else
        {
            Debug.Log("No robot in that slot");
        }
    }

    public void BuyRobot(GameObject robotPrefab, RobotAttributes attributes)
    {
        if (unassignedRobots.Count < 4)
        {
            RobotInfo newRobot = new RobotInfo
            {
                robotPrefab = robotPrefab,
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
