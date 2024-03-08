using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformStats
{
    [SerializeField] public bool isAssigned = false;
    
    [SerializeField] public bool hasRobotPrefab = false;
    [SerializeField] private int quickSellPrice;
    [SerializeField] public String robotType;
    [SerializeField] public String size;
    [SerializeField] private float cps;


    public PlatformStats(bool isAssigned, RobotAttributes attributes)
    {
        this.isAssigned = isAssigned;
        
        if (isAssigned && !hasRobotPrefab)
        {
            quickSellPrice = attributes.quickSellPrice;
            robotType = attributes.robotType;
            size = attributes.size;
            cps = attributes.cps;
        }
        
    }
}
