using UnityEngine;

public class PlotStats : MonoBehaviour
{
    [SerializeField] public bool isLocked = true;
    [SerializeField] public BoxCollider boundryPos;
    [SerializeField] public Material lockMaterial;
    [SerializeField] public Material unlockMaterial;
    [SerializeField ]public Component lockItem;
    [SerializeField ]public Component unLockedPlot;
    
    public Component currentlockItem;
    public Component currentunlockItem;
    public float PlotPrice { get; private set; }

    #region Unity Callbacks

    private void Awake()
    {
        SetPlotPrice();
    }

    #endregion

    #region Plot Configuration

    public void SetPlotColor(bool locked)
    {
        var renderer = gameObject.GetComponent<MeshRenderer>();
        
        renderer.material = locked ? lockMaterial : unlockMaterial;
        if (locked)
        {
            if (currentlockItem == null)
            {
                currentlockItem = Instantiate(lockItem, transform);
            }
            
        }
        else
        {
            if (currentunlockItem == null)
            {
                currentunlockItem = Instantiate(unLockedPlot, transform);
            }
        }
    }

    public void SetPlotPrice()
    {
        float distanceFromStartingPlot = Mathf.Abs(Mathf.RoundToInt(transform.position.x) / 8);
        float basePrice = 500;
        float pricePerDistance = 100;

        PlotPrice = basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance, 8 / 5F));
    }

    #endregion

    #region Boundry Operations

    public void ActivateBoundry()
    {
        var renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = lockMaterial;
        boundryPos.enabled = true;
    }

    public void DeactivateBoundry()
    {
        var renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = unlockMaterial;
        if (currentlockItem != null)
        {
            Destroy(currentlockItem.gameObject);
        }
        if (currentunlockItem == null)
        {
            currentunlockItem = Instantiate(unLockedPlot, transform);
        }
        boundryPos.enabled = false;
    }

    #endregion
}