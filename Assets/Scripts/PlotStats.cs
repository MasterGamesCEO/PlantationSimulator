using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlotStats : MonoBehaviour
{
    [SerializeField] public Boolean isLocked = true;
    [SerializeField] public BoxCollider boundryPos;


    public void deactivateBoundry()
    {
        boundryPos.enabled = false;
    }
}
