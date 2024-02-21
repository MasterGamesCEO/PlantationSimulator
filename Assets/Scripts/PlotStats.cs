using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlotStats : MonoBehaviour
{
    [SerializeField] public Boolean isLocked = true;
    [SerializeField] public BoxCollider boundryPos;
    [SerializeField] public int plotPrice;
    [SerializeField] public GameObject selectedPlot;
    
    private void Start()
    {
        setPrice();
    }
    
    

    private void setPrice()
    {
        
    }


    public void deactivateBoundry()
    {
        boundryPos.enabled = false;
    }
}
