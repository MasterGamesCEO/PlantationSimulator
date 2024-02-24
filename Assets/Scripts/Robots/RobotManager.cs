using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [SerializeField] public List<RobotInfo> availableRobots = new List<RobotInfo>();
    [SerializeField] public List<AssignedRobotInfo> assignedRobots = new List<AssignedRobotInfo>();

    [System.Serializable]
    public class RobotInfo
    {
        public GameObject robotPrefab;
        public RobotAttributes attributes; // Add information about the robot
    }

    [System.Serializable]
    public class AssignedRobotInfo
    {
        public GameObject robotInstance;
        public GameObject assignedPlot;
        public RobotAttributes attributes; // Store the attributes of the assigned robot
    }

    public void AssignRobotToPlot(GameObject plot, int selectedRobotIndex)
    {
        // Check if the selected index is valid
        if (selectedRobotIndex >= 0 && selectedRobotIndex < availableRobots.Count)
        {
            // Get the selected robot
            RobotInfo selectedRobotInfo = availableRobots[selectedRobotIndex];

            // Instantiate the selected robot on the plot
            GameObject newRobot = Instantiate(selectedRobotInfo.robotPrefab, plot.transform.position, Quaternion.identity);

            // Remove the selected robot from available list
            availableRobots.RemoveAt(selectedRobotIndex);

            // Add the assigned robot to the list
            assignedRobots.Add(new AssignedRobotInfo
            {
                robotInstance = newRobot,
                assignedPlot = plot,
                attributes = selectedRobotInfo.attributes
            });
        }
        else
        {
            Debug.Log("Invalid selected robot index");
        }
    }

    public void BuyRobot(GameObject robotPrefab, RobotAttributes attributes)
    {
        RobotInfo newRobot = new RobotInfo
        {
            robotPrefab = robotPrefab,
            attributes = attributes
        };

        // Check if the maximum number of available robots is reached
        if (availableRobots.Count < 4)
        {
            availableRobots.Add(newRobot);
        }
        else
        {
            Debug.Log("Maximum number of available robots reached");
        }
    }

    // Additional methods and functionalities can be added as needed
}
