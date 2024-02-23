using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
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
        robotValue();
        
    }

    private void robotValue()
    {
        if (CPS >= 0.001 & CPS < 0.1)
        {
            robotType.Equals("basicRobot");
            basicRobotS();
        }
        if (CPS >= 0.1 & CPS < 1)
        {
            robotType.Equals("silverRobot");
            silverRobotS();
        }
        if (CPS >= 1 & CPS < 2)
        {
            robotType.Equals("goldRobot");
            goldRobotS();
        }
        if (CPS >= 2 & CPS < 3.5)
        {
            robotType.Equals("diamondRobot");
            diamondRobotS();
        }
        if (CPS >= 3.5 & CPS < 5)
        {
            robotType.Equals("ultraBot");
            ultraBotS();
        }
        
    }

    private void basicRobotS()
    {
        price = 20;
        quickSellPrice = 1;
        robotType = "basicRobot";
        
        size = CPS >= 0.05 ? "scrawny" : "normal";
    }

    private void silverRobotS()
    {
        price = 100;
        quickSellPrice = 10;
        robotType = "silverRobot";
        
        size = CPS >= .5 ? "normal" : "tall";
        
    }

    private void goldRobotS()
    {
        price = 500;
        quickSellPrice = 50;
        robotType = "goldRobot";
        
        size = CPS >= 1.5 ? "tall" : "built";
    }

    private void diamondRobotS()
    {
        price = 2000;
        quickSellPrice = 100;
        robotType = "diamondRobot";
        
        size = CPS >= 2.9 ? "built" : "massive";
    }

    private void ultraBotS()
    {
        price = 10000;
        quickSellPrice = 1000;
        robotType = "ultraBot";
        
        size = CPS >= 4 ? "massive" : "tank";
    }
    private void Update()
    {
        
    }
}