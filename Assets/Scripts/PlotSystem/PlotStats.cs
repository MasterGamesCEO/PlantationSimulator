using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlotStats : MonoBehaviour
{
    [SerializeField] public bool isLocked = true;
    [SerializeField] public BoxCollider boundryPos;
    [FormerlySerializedAs("LockPrefab")] [SerializeField] public Component lockPrefab;
    [FormerlySerializedAs("UnlockedPrefab")] [SerializeField] public Component unlockedPrefab;
    
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
        float basePrice = 500;
        float pricePerDistance = 100;

        PlotPrice = basePrice + (Mathf.Pow(distanceFromStartingPlot * pricePerDistance, 8 / 5F));
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