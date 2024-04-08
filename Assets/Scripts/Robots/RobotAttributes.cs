using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[System.Serializable]
public class RobotAttributes 
{
    [SerializeField] public float cps;
    [SerializeField] public float price;
    [SerializeField] public float quickSellPrice;
    [SerializeField] public float ID;
    [SerializeField] public String robotType;
    [SerializeField] public String size;
}