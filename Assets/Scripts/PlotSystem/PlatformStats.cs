using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformStats
{
    [SerializeField] public bool isAssigned = false;
    
    [SerializeField] public bool hasRobotPrefab = false;
    
    [SerializeField] public float quickSellPrice;
    [SerializeField] public String robotType;
    [SerializeField] public String size;
    [SerializeField] public float cps;
    [SerializeField] public float price;
    [SerializeField] public float ID;


    public PlatformStats(bool isAssigned, RobotAttributes attributes)
    {
        this.isAssigned = isAssigned;
        
        if (isAssigned && !hasRobotPrefab)
        {
            quickSellPrice = attributes.quickSellPrice;
            robotType = attributes.robotType;
            size = attributes.size;
            cps = attributes.cps;
            price = attributes.price;
            ID = attributes.ID;
        }
        
    }
}
