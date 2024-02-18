using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlotPricePopupScript : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI plotPrice;
    [SerializeField] TextMeshProUGUI moneyText;

    private PlayerController _playerController;
    private Animator _mAnimator;

    private static readonly int Popup = Animator.StringToHash("popup");

    private void Start()
    {
        _mAnimator = GetComponent<Animator>();
        _playerController = FindObjectOfType<PlayerController>();
        UpdateMoney(0);
    }

    public void ActivatePopup(float plotPriceValue)
    {
        plotPriceValue = (int)plotPriceValue;
        plotPrice.text = plotPriceValue.ToString();
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

    public void UpdateMoney(float moneyDelta)
    {
        if (_playerController != null)
        {
            var moneyAfterUpdate = (int)_playerController.GetPlayerMoney() - (int)moneyDelta;
            moneyText.text = "$" + moneyAfterUpdate.ToString();
        }
        else
        {
            Debug.LogError("PlayerController not found!");
        }
    }
}