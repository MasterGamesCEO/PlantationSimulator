using System;
using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SellCrops : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textBoxPrice;
    [SerializeField] public int stockPrice;
    [SerializeField] public float RandyTime;
    
    void Update()
    {
        if (RandyTime > 0)
        {
            RandyTime -= Time.deltaTime;
        }
        else
        {
            RandyTime = Random.Range(60, 600); //1-10 minute random
            stockPrice = Random.Range(50, 200);
            textBoxPrice.text = stockPrice.ToString("C0");
        }
        
    }


    public void SellCropsForMoney(float cropsToSell)
    {
        CurrentData.Instance.uiData.saveMoney += cropsToSell * stockPrice;
        CurrentData.Instance.uiData.cropData -= cropsToSell;
        FindObjectOfType<PlotPricePopupScript>().UpdateMoney(0);
        FindObjectOfType<NumberCounter>().Value -= cropsToSell;
    }

}
