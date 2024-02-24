using UnityEngine;

public class PlotStats : MonoBehaviour
{
    [SerializeField] public bool isLocked = true;
    [SerializeField] public BoxCollider boundryPos;
    [SerializeField] public Component LockPrefab;
    [SerializeField] public Component UnlockedPrefab;
    
    private Component _currentLockItem;
    private Component _currentUnlockItem;
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
        if (locked)
        {
            if (_currentLockItem == null)
            {
                _currentLockItem = Instantiate(LockPrefab, transform);
            }
            
        }
        else
        {
            if (_currentUnlockItem == null)
            {
                _currentUnlockItem = Instantiate(UnlockedPrefab, transform);
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
        boundryPos.enabled = true;
    }

    public void DeactivateBoundry()
    {
        if (_currentLockItem != null)
        {
            Destroy(_currentLockItem.gameObject);
        }
        if (_currentUnlockItem == null)
        {
            _currentUnlockItem = Instantiate(UnlockedPrefab, transform);
        }
        boundryPos.enabled = false;
    }

    #endregion
}