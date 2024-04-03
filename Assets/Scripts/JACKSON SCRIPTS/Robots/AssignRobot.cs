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
    [NonSerialized] public bool isAssignable = false;
    private Vector3 robotPosition;
    
    void Start()
    {
        if (isAssignable == true)
        {
            Debug.Log("Assignable");
            // todo later: make the code run on a button click
            RobotAttributes makePlot = new RobotAttributes();
            selectedRobot.transform.position.Equals(assigningPlot.transform.position);
            

        }
        if (isAssignable == false)
        {
            Debug.Log("Nothing");
        }
    }
   
    private void assigned()
    {
        
       
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
