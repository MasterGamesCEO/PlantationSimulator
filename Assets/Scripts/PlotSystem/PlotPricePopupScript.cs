using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlotPricePopupScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI plotPrice;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject moneySpreadPrefab;
    
    private GameObject _currentMoneySpread;
    [SerializeField] private PlayerController playerController;
    private SaveData _saveData;
    private Animator _mAnimator;

    private static readonly int Popup = Animator.StringToHash("popup");

    #region Unity Callbacks

    private void Start()
    {
        _saveData = SaveData.Instance;
        _mAnimator = GetComponent<Animator>();
        UpdateMoney(0);
    }

    #endregion

    #region Popup Management

    public void ActivatePopup(float plotPriceValue)
    {
        plotPriceValue = (int)plotPriceValue;
        plotPrice.text = "$" + plotPriceValue.ToString();
        _mAnimator.SetBool(Popup, true);
    }

    public void DeactivatePopup()
    {
        _mAnimator.SetBool(Popup, false);
    }

    public bool PopupActive()
    {
        return _mAnimator.GetBool(Popup);
    }

    #endregion

    #region Money Operations

    // ReSharper disable Unity.PerformanceAnalysis
    public void UpdateMoney(float moneyDelta)
    {
        _saveData = FindObjectOfType<SaveData>();
        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            float moneyAfterUpdate = _saveData.playerMoney - moneyDelta;
            moneyText.text = "$" + ((int)moneyAfterUpdate).ToString();
            Debug.Log("Updated money $" + moneyAfterUpdate);
        }
    }

    public void RunMoneySpread()
    {
        Destroy(_currentMoneySpread);
        _currentMoneySpread = Instantiate(moneySpreadPrefab, playerController.transform);
    }

    #endregion
}