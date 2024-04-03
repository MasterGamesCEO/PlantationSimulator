using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AssignRobots : MonoBehaviour
{
    [SerializeField] private GameObject selectedRobot;
    [SerializeField] private GameObject assigningPlot;
    [NonSerialized] public bool IsAssignable = false;
    private Vector3 _robotPosition;
    
    void Start()
    {
        if (IsAssignable == true)
        {
            Debug.Log("Assignable");
            // todo later: make the code run on a button click
            RobotAttributes makePlot = new RobotAttributes();
            selectedRobot.transform.position.Equals(assigningPlot.transform.position);
            

        }
        if (IsAssignable == false)
        {
            Debug.Log("Nothing");
        }
    }
   
    private void Assigned()
    {
        
       
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
