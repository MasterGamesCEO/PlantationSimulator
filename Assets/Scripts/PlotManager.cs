using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{

    [SerializeField] private static GameObject plot1;
    [SerializeField] private static GameObject plot2;
    [SerializeField] private static GameObject plot3;
    [SerializeField] private static GameObject plot4;
    [SerializeField] private static GameObject plot5;
    [SerializeField] private static GameObject plot6;
    [SerializeField] private static GameObject plot7;
    [SerializeField] private static GameObject plot8;
    [SerializeField] private static GameObject plot9;
    
    public GameObject[ ] row1 = new GameObject[ ]{plot1, plot3, plot9} ;
    public GameObject[ ] row2 = new GameObject[ ]{plot8, plot2, plot4} ;
    public GameObject[ ] row3 = new GameObject[ ]{plot5, plot6, plot7} ;
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {

    }
 
    // Update is called once per frame
    void Update()
    {
        
    }
}

