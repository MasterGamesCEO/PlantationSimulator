using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StockMarket : MonoBehaviour
{

    public float timeStart = 200;
    [FormerlySerializedAs("StockPrice")] [SerializeField] private int stockPrice = 100;
    private string _countDown;
    private bool _timesUp = false;
    private bool _stockChange = false;

    private void Stocks()
    {
        if (timeStart <= 0)
        {
            _timesUp = true;
        }
        if (_timesUp == true)
        {
            ResetTimer();
            _stockChange = true;
        }
        if (_stockChange == true)
        {
            stockPrice = Random.Range(50, 200);
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
        _countDown = Mathf.Round(timeStart).ToString();
        if ( timeStart < 0 )
        {
            Stocks();
        }
        
    }
}
