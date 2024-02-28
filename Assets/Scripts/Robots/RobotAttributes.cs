using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Search;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RobotAttributes : MonoBehaviour
{
    [SerializeField] private float cps;
    [SerializeField] private int price;
    [SerializeField] private int quickSellPrice;
    [SerializeField] public String robotType;
    [SerializeField] public String size;
    

    private void Start()
    {
        cps = Random.Range(0.001f, 5f);
        RobotValue();
        
    }
    public void SaveAttributes(string prefix)
    {
        PlayerPrefs.SetString($"{prefix}_RobotType", robotType);
        PlayerPrefs.SetString($"{prefix}_size", size);
        PlayerPrefs.SetFloat($"{prefix}_cps", cps);
        PlayerPrefs.SetInt($"{prefix}_price", price);
    }

    public void LoadAttributes(string prefix)
    {
        robotType = PlayerPrefs.GetString($"{prefix}_RobotType", "");
        Debug.Log("Attributes loaded");
        size = PlayerPrefs.GetString($"{prefix}_size", "");
        cps = PlayerPrefs.GetFloat($"{prefix}_Speed", 0f);
        price = PlayerPrefs.GetInt($"{prefix}_price", 0);
    }

    private void RobotValue()
    {
        if (cps >= 0.001 & cps < 0.1)
        {
            robotType.Equals("basicRobot");
            BasicRobotS();
        }
        if (cps >= 0.1 & cps < 1)
        {
            robotType.Equals("silverRobot");
            SilverRobotS();
        }
        if (cps >= 1 & cps < 2)
        {
            robotType.Equals("goldRobot");
            GoldRobotS();
        }
        if (cps >= 2 & cps < 3.5)
        {
            robotType.Equals("diamondRobot");
            DiamondRobotS();
        }
        if (cps >= 3.5 & cps < 5)
        {
            robotType.Equals("ultraRobot");
            UltraBotS();
        }
        
    }

    private void BasicRobotS()
    {
        price = 20;
        quickSellPrice = 1;
        robotType = "basicRobot";
        
        size = cps >= 0.05 ? "scrawny" : "normal";
    }

    private void SilverRobotS()
    {
        price = 100;
        quickSellPrice = 10;
        robotType = "silverRobot";
        
        size = cps >= .5 ? "normal" : "tall";
        
    }

    private void GoldRobotS()
    {
        price = 500;
        quickSellPrice = 50;
        robotType = "goldRobot";
        
        size = cps >= 1.5 ? "tall" : "built";
    }

    private void DiamondRobotS()
    {
        price = 2000;
        quickSellPrice = 100;
        robotType = "diamondRobot";
        
        size = cps >= 2.9 ? "built" : "massive";
    }

    private void UltraBotS()
    {
        price = 10000;
        quickSellPrice = 1000;
        robotType = "ultraRobot";
        
        size = cps >= 4 ? "massive" : "tank";
    }
}