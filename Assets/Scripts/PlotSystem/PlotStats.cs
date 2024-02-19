using UnityEngine;

public class PlotStats : MonoBehaviour
{
    [SerializeField] public bool isLocked = true;
    [SerializeField] public BoxCollider boundryPos;
    [SerializeField] public Material lockMaterial;
    [SerializeField] public Material unlockMaterial;
    public float PlotPrice { get; private set; }

    #region Unity Callbacks

    private void Awake()
    {
        SetPlotPrice();
    }

    #endregion

    #region Plot Configuration

    public void setPlotColor(bool locked)
    {
        var renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = locked ? lockMaterial : unlockMaterial;
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
        boundryPos.enabled = false;
    }

    #endregion
}