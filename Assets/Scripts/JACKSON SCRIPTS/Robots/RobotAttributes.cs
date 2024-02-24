using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Search;
using Random = UnityEngine.Random;

public class RobotAttributes : MonoBehaviour
{
    [SerializeField] private float CPS;
    [SerializeField] public GameObject assignedPlot;
    [SerializeField] private int price;
    [SerializeField] private int quickSellPrice;
    [SerializeField] private String robotType;
    [SerializeField] private String size;

    private void Start()
    {
        CPS = Random.Range(0.001f, 5f);
        RobotValue();
        
    }

    private void RobotValue()
    {
        if (CPS >= 0.001 & CPS < 0.1)
        {
            robotType.Equals("basicRobot");
            BasicRobotS();
        }
        if (CPS >= 0.1 & CPS < 1)
        {
            robotType.Equals("silverRobot");
            SilverRobotS();
        }
        if (CPS >= 1 & CPS < 2)
        {
            robotType.Equals("goldRobot");
            GoldRobotS();
        }
        if (CPS >= 2 & CPS < 3.5)
        {
            robotType.Equals("diamondRobot");
            DiamondRobotS();
        }
        if (CPS >= 3.5 & CPS < 5)
        {
            robotType.Equals("ultraBot");
            UltraBotS();
        }
        
    }

    private void BasicRobotS()
    {
        price = 20;
        quickSellPrice = 1;
        robotType = "basicRobot";
        
        size = CPS >= 0.05 ? "scrawny" : "normal";
    }

    private void SilverRobotS()
    {
        price = 100;
        quickSellPrice = 10;
        robotType = "silverRobot";
        
        size = CPS >= .5 ? "normal" : "tall";
        
    }

    private void GoldRobotS()
    {
        price = 500;
        quickSellPrice = 50;
        robotType = "goldRobot";
        
        size = CPS >= 1.5 ? "tall" : "built";
    }

    private void DiamondRobotS()
    {
        price = 2000;
        quickSellPrice = 100;
        robotType = "diamondRobot";
        
        size = CPS >= 2.9 ? "built" : "massive";
    }

    private void UltraBotS()
    {
        price = 10000;
        quickSellPrice = 1000;
        robotType = "ultraBot";
        
        size = CPS >= 4 ? "massive" : "tank";
    }
}