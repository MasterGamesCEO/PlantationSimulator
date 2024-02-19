using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBoxPrefab;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlotStats[] allPlots;
    [SerializeField] private PlotPricePopupScript _plotPricePopupScript;

    private InputManager _input;
    private GameObject _currentDialogBox;
    private int _selectedOptionIndex = 0;
    private bool _isDialogOpen = false;
    private bool _navigationProcessedThisFrame = false;
    private bool _wasYPressed = false;

    private const float DefaultPlayerPositionX = 0f;
    private const float DefaultPlayerPositionY = 1f;
    private const float DefaultPlayerPositionZ = 0f;

    private void Awake()
    {
        // Check if playerController is assigned before calling methods on it
        if (playerController != null)
        {
            playerController.SetPlayerMoney(10000);
            _plotPricePopupScript.UpdateMoney(0);
        }
    }

    private void Start()
    {
        _input = InputManager.Instance;
        _input.openDialog.performed += OnDialogPerformed;

        // Load saved game data
        LoadGameData();
        
    
        // Unlock the first plot immediately
        if (allPlots.Length > 0)
        {
            allPlots[0].isLocked = false;
        }
    }

    private void Update()
    {
        if (_isDialogOpen)
        {
            Vector2 uiInput = _input.uiControls.ReadValue<Vector2>();
            HandleNavigation(uiInput);
            if (_input.enterKey.triggered)
            {
                ExecuteSelectedAction();
            }
            UpdateUI();
        }
        
        _navigationProcessedThisFrame = false;
    }

    #region Actions

    private void ExecuteSelectedAction()
    {
        switch (_selectedOptionIndex)
        {
            case 0:
                OnResumeAction();
                break;
            case 1:
                OnSaveAction();
                break;
            case 2:
                OnSettingsAction();
                break;
        }
    }

    private void OnResumeAction()
    {
        CloseDialogBox();
    }

    private void OnSaveAction()
    {
        SaveGameData();
        Application.Quit();
    }

    private void OnSettingsAction()
    {
        ResetGameData();
    }

    #endregion

    #region UIPOPUP AND NAV

    private void HandleNavigation(Vector2 uiInput)
    {
        if (_navigationProcessedThisFrame)
            return;

        if (uiInput.y > 0 && !_wasYPressed)
        {
            _selectedOptionIndex = (_selectedOptionIndex - 1 + 3) % 3;
            _navigationProcessedThisFrame = true;
            _wasYPressed = true;
        }
        else if (uiInput.y < 0 && !_wasYPressed)
        {
            _selectedOptionIndex = (_selectedOptionIndex + 1) % 3;
            _navigationProcessedThisFrame = true;
            _wasYPressed = true;
        }
        else if (uiInput.y == 0)
        {
            _wasYPressed = false;
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform option = _currentDialogBox.transform.Find($"Canvas/Foreground/Options/Option{i}");
            if (option != null)
            {
                TextMeshProUGUI optionText = option.Find("Text")?.GetComponent<TextMeshProUGUI>();
                Transform fillImageTransform = option.Find("Fill");

                if (fillImageTransform != null)
                {
                    fillImageTransform.gameObject.SetActive(i == _selectedOptionIndex);
                }

                if (optionText != null)
                {
                    optionText.fontStyle = i == _selectedOptionIndex ? FontStyles.Bold : FontStyles.Normal;
                }
            }
        }
    }

    private void OnDialogPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Dialog performed!");
        _isDialogOpen = !_isDialogOpen;

        if (_isDialogOpen)
        {
            OpenDialogBox();
        }
        else
        {
            CloseDialogBox();
        }
    }

    private void OpenDialogBox()
    {
        Destroy(_currentDialogBox);
        _currentDialogBox = Instantiate(dialogBoxPrefab, transform);
        _input.uiControls.Enable();
        _input.movement.Disable();
    }

    private void CloseDialogBox()
    {
        if (_currentDialogBox != null)
        {
            Destroy(_currentDialogBox);
        }

        _isDialogOpen = false;
        _input.uiControls.Disable();
        _input.movement.Enable();
    }

    #endregion

    #region Save and Load Game Data

    private void SaveGameData()
    {
        PlayerPrefs.SetFloat("PlayerMoney", playerController.GetPlayerMoney());
        PlayerPrefs.SetFloat("PlayerPositionX", playerController.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", playerController.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", playerController.transform.position.z);

        for (int i = 0; i < allPlots.Length; i++)
        {
            PlayerPrefs.SetInt($"Plot_{i}_IsLocked", allPlots[i].isLocked ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    private void LoadGameData()
    {
        playerController.SetPlayerMoney(PlayerPrefs.GetFloat("PlayerMoney", 10000f));
        _plotPricePopupScript.UpdateMoney(0);
        // Load player's position
        float playerPositionX = PlayerPrefs.GetFloat("PlayerPositionX", DefaultPlayerPositionX);
        float playerPositionY = PlayerPrefs.GetFloat("PlayerPositionY", DefaultPlayerPositionY);
        float playerPositionZ = PlayerPrefs.GetFloat("PlayerPositionZ", DefaultPlayerPositionZ);

        // Set the player's position immediately
        playerController.transform.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);
        Debug.Log("Loaded Player Position: " + playerController.transform.position);
        
        
        // Load plot status
        for (int i = 0; i < allPlots.Length; i++)
        {
            int isLockedValue = PlayerPrefs.GetInt($"Plot_{i}_IsLocked", 1);
            allPlots[i].isLocked = isLockedValue == 1;
            allPlots[i].setPlotColor(allPlots[i].isLocked = isLockedValue == 1);
        }
    }

    private void UnlockFirstPlot()
    {
        if (allPlots.Length > 0)
        {
            allPlots[0].isLocked = false;
            allPlots[0].DeactivateBoundry();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ResetGameData()
    {
        playerController.SetPlayerMoney(10000);
        _plotPricePopupScript.UpdateMoney(0);

        for (int i = 0; i < allPlots.Length; i++)
        {
            allPlots[i].isLocked = true;
            allPlots[i].ActivateBoundry();
            
        }
        UnlockFirstPlot();
        

        PlayerPrefs.DeleteAll();
        playerController.transform.position = new Vector3(DefaultPlayerPositionX, DefaultPlayerPositionY, DefaultPlayerPositionZ);
        
    }

    
    #endregion
}
