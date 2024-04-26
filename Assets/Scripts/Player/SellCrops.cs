using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using UnityEngine;

public class SellCrops : MonoBehaviour
{
    [SerializeField] public int stockPrice = 100;
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
