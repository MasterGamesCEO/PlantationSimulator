using UnityEngine;

public class PlotStats : MonoBehaviour
{
    [SerializeField] public bool isLocked = true;
    [SerializeField] public BoxCollider boundryPos;
    [SerializeField ]public Material LockMaterial;
    [SerializeField ]public Material unlockMaterial;
    public float PlotPrice { get; private set; }

    private void Awake()
    {
        SetPlotPrice();
    }

    public void setPlotColor(bool locked)
    {
        if (locked)
        {
            gameObject.GetComponent<MeshRenderer>().material = LockMaterial;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = unlockMaterial;
        }
    }

    public void SetPlotPrice()
    {
        float distanceFromStartingPlot = Mathf.Abs(Mathf.RoundToInt(transform.position.x)/8);
        float basePrice = 500; 
        float pricePerDistance = 100; 
        
        PlotPrice = basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance,8/5F));
        
    }

    public void ActivateBoundry()
    {
        gameObject.GetComponent<MeshRenderer>().material = LockMaterial;
        boundryPos.enabled = true;
    }

    public void DeactivateBoundry()
    {
        gameObject.GetComponent<MeshRenderer>().material = unlockMaterial;
        boundryPos.enabled = false;
    }
}