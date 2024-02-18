using UnityEngine;

public class PlotStats : MonoBehaviour
{
    [SerializeField] public bool isLocked = true;
    [SerializeField] public BoxCollider boundryPos;
    
    public float PlotPrice { get; private set; }

    private void Awake()
    {
        SetPlotPrice();
    }

    public void SetPlotPrice()
    {
        float distanceFromStartingPlot = Mathf.Abs(Mathf.RoundToInt(transform.position.x)/8);
        float basePrice = 500; 
        float pricePerDistance = 100; 
        
        PlotPrice = basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance,8/5F));
        
    }

    public void DeactivateBoundry()
    {
        boundryPos.enabled = false;
    }
}