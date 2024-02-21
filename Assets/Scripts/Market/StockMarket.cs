using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockMarket : MonoBehaviour
{

    public float timeStart = 200;
    [SerializeField] private int StockPrice = 100;
    private string countDown;
    private bool timesUp = false;
    private bool stockChange = false;

    private void Stocks()
    {
        if (timeStart <= 0)
        {
            timesUp = true;
        }
        if (timesUp == true)
        {
            ResetTimer();
            stockChange = true;
        }
        if (stockChange = true)
        {
            StockPrice = Random.Range(50, 200);
        }
    }
    private void ResetTimer()
    {
        timeStart = 200;

    }
    
   
    
    // Update is called once per frame
    void Update()
    {
        timeStart -= Time.deltaTime;
        countDown = Mathf.Round(timeStart).ToString();
        if ( timeStart < 0 )
        {
            Stocks();
        }
        
    }
}
