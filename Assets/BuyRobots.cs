using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyRobots : MonoBehaviour
{
    private AuctionScript _auctionScript;
    private SaveData _saveData;
    private InputManager _input;
    private bool _navigationProcessedThisFrame = false;
    private bool _wasXPressed = false;
    private int _selectedOptionIndexStart = 1;
    void Start()
    {
       _auctionScript = FindObjectOfType<AuctionScript>();
       _saveData = SaveData.Instance;
       _input = InputManager.Instance;
       _input.EnableAuctionControls();
    }

    private void Update()
    {
        Vector2 uiInput = _input.auctionControls.ReadValue<Vector2>();

        if (_auctionScript.IsPopupActive())
        {
            HandleStartNavigation(uiInput);
            if (_input.enterKey.triggered)
            {
                ExecuteStartSelectedAction();
            }

            UpdateStartUI();
        }
        _navigationProcessedThisFrame = false;
    }
    private void HandleStartNavigation(Vector2 uiInput)
    {
        if (_navigationProcessedThisFrame)
            return;

        if (uiInput.x < 0 && !_wasXPressed)
        {
            _selectedOptionIndexStart = (_selectedOptionIndexStart - 1 + 3) % 3;
            _navigationProcessedThisFrame = true;
            _wasXPressed = true;
        }
        else if (uiInput.x > 0 && !_wasXPressed)
        {
            _selectedOptionIndexStart = (_selectedOptionIndexStart + 1) % 3;
            _navigationProcessedThisFrame = true;
            _wasXPressed = true;
        }
        else if (uiInput.x == 0)
        {
            _wasXPressed = false;
        }
    }
    private void UpdateStartUI()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform option = transform.Find($"Canvas/PANEL{i}");
            if (option != null)
            {
                TextMeshProUGUI optionTYPE = option.Find("FORE/TYPE")?.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI optionSIZE = option.Find("FORE/SIZE")?.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI optionPRICE = option.Find("FORE/PRICE")?.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI optionCPS = option.Find("FORE/CPS")?.GetComponent<TextMeshProUGUI>();
                Transform fillImageTransform = option.Find("FORE/OPTION/FILL");

                if (fillImageTransform != null)
                {
                    fillImageTransform.gameObject.SetActive(i == _selectedOptionIndexStart);
                }

                if ((optionTYPE != null )&& (optionSIZE != null) && (optionPRICE != null) && (optionCPS != null))
                {
                    optionTYPE.fontStyle = i == _selectedOptionIndexStart ? FontStyles.Bold : FontStyles.Normal;
                    optionSIZE.fontStyle = i == _selectedOptionIndexStart ? FontStyles.Bold : FontStyles.Normal;
                    optionCPS.fontStyle = i == _selectedOptionIndexStart ? FontStyles.Bold : FontStyles.Normal;
                    optionPRICE.fontStyle = i == _selectedOptionIndexStart ? FontStyles.Bold : FontStyles.Normal;
                }
            }
        }
    }
    private void ExecuteStartSelectedAction()
    {
        RobotManager manager = FindObjectOfType<RobotManager>();
        if (_selectedOptionIndexStart == 0)
        {
            manager.BuyRobot(manager.auctionRobots[0]);
        }
        else if(_selectedOptionIndexStart == 1)
        {
            manager.BuyRobot(manager.auctionRobots[1]);
        }
        else if (_selectedOptionIndexStart == 2)
        {
            manager.BuyRobot(manager.auctionRobots[2]);
        }
    }
}
