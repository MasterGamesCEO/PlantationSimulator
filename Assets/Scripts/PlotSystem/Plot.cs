using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class Plot : MonoBehaviour
{

    [SerializeField] public PlotStats stats;
    [SerializeField] public BoxCollider boundryPos;
    [SerializeField] public Component lockPrefab;
    [SerializeField] public Component unlockedPrefab;
    
    private Component _currentLockItem;
    private Component _currentUnlockItem;
    public float PlotPrice { get; private set; }

    #region Unity Callbacks

    private void Awake()
    {
        SetPlotPrice();
    }
    
    #endregion

    public void SetPlotStats(PlotStats loadedStats)
    {
        stats = loadedStats;
    }

    public PlotStats GetPlotStats()
    {
        return stats;
    }

    #region Plot Configuration

    public void SetPlotColor(bool locked)
    {
        if (locked)
        {
            if (_currentLockItem == null)
            {
                _currentLockItem = Instantiate(lockPrefab, transform);
            }
            
        }
        else
        {
            if (_currentUnlockItem == null)
            {
                _currentUnlockItem = Instantiate(unlockedPrefab, transform);
            }
        }
    }

    public void SetPlotPrice()
    {
        float distanceFromStartingPlot = Mathf.Abs(Mathf.RoundToInt(transform.position.x) / 8);
        float basePrice = 2000;
        float pricePerDistance = 100;

        
        if ((basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance, 2)) < 92000))
        {
            PlotPrice = (int)(basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance, 2)));
        }
        else if ((basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance, 2.7f)) > 20000000))
        {
            PlotPrice = (int)(basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance, 3.2f)));
        } else if ((basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance, 2)) < 252000))
        {
            PlotPrice = (int)(basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance, 2.2f)));
        }
        else
        {
            PlotPrice = (int)(basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance, 2.7f)));
        } 
    }

    #endregion

    #region Boundry Operations

    public void ActivateBoundary()
    {
        boundryPos.enabled = true;
    }

    public void DeactivateBoundary()
    {
        if (_currentLockItem != null)
        {
            Destroy(_currentLockItem.gameObject);
        }
        if (_currentUnlockItem == null)
        {
            _currentUnlockItem = Instantiate(unlockedPrefab, transform);
        }
        boundryPos.enabled = false;
    }

    #endregion
}