using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UnassignPopup : MonoBehaviour
{
    private Animator _mAnimator;
    [SerializeField] public Platform selectedPlatform;
    private static readonly int In = Animator.StringToHash("In");
    private InputManager _input;

    // Start is called before the first frame update
    void Start()
    {
        _input = InputManager.Instance;
        _mAnimator = GetComponent<Animator>();
        _input.quickSell.performed += OnUnAssignRobotPreformed;
        _input.unAssignRobot.performed += OnUnAssignRobotPreformed;
    }

    private void OnUnAssignRobotPreformed(InputAction.CallbackContext obj)
    {
        if (_input.quickSell.WasPressedThisFrame()) 
        {
            if (PopupActive())
            {
                Debug.Log(selectedPlatform);
                RobotManager manager = FindObjectOfType<RobotManager>();
                PlatformDataHandler handler = FindObjectOfType<PlatformDataHandler>();
                if (manager != null && handler != null)
                {
                    //TODO: Remove from working robots the stats
                }
            }
        } else if (_input.unAssignRobot.WasPressedThisFrame())
        {
            //TODO: move the stats back to unassigned
        }

        
    }

    public void ActivatePopup(Platform platform)
    {
        RobotManager manager = FindObjectOfType<RobotManager>();
        Transform option = _mAnimator.transform.Find($"Slot{0}");
        TextMeshProUGUI optionText = option.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();
        if (optionText != null)
            optionText.text = "$" + platform.stats.quickSellPrice;

        _mAnimator.SetBool(In, true);
        selectedPlatform = platform;
    }

    public void DeactivatePopup()
    {
        _mAnimator.SetBool(In, false);
        selectedPlatform = null;
    }

    public bool PopupActive()
    {
        return _mAnimator.GetBool(In);
    }
}
