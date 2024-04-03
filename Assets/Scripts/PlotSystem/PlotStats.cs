using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlotStats
{
    [SerializeField] public bool isLocked = true;


    public PlotStats(bool isLocked)
    {
        this.isLocked = isLocked;
    }
}
