using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private GameObject savePopupPrefab;
    [SerializeField] private GameObject dialogBoxPrefab;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlotPricePopupScript _plotPricePopupScript;

    private InputManager _input;
    private GameObject _currentSaveBox;
    private GameObject _currentDialogBox;
    private int _selectedOptionIndex = 0;
    private int _selectedOptionIndexSave = 0;
    private bool _isDialogOpen = false;
    private bool _isSaveOpen = false;
    private bool _navigationProcessedThisFrame = false;
    private bool _wasYPressed = false;
    private bool _wasXPressed = false;

    private const float DefaultPlayerPositionX = 0f;
    private const float DefaultPlayerPositionY = 1f;
    private const float DefaultPlayerPositionZ = 0f;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
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

        LoadGameData();

        PlotDataHandler plotDataHandler = FindObjectOfType<PlotDataHandler>();
        if (plotDataHandler != null)
        {
            plotDataHandler.UnlockFirstPlot();
        }
    }

    private void Update()
    {
        if (_isDialogOpen && !_isSaveOpen)
        {
            Vector2 uiInput = _input.uiControls.ReadValue<Vector2>();
            HandleNavigation(uiInput);
            if (_input.enterKey.triggered)
            {
                ExecuteSelectedAction();
            }
            UpdateUI();
        }
        else if(_isDialogOpen && _isSaveOpen)
        {
            Vector2 uiInput = _input.uiControls.ReadValue<Vector2>();
            HandleSaveNavigation(uiInput);
            if (_input.enterKey.triggered)
            {
                ExecuteSelectedSaveAction();
            }
            UpdateSaveUI();
        }

        _navigationProcessedThisFrame = false;
    }

    #endregion

    #region Actions

    private void ExecuteSelectedAction()
    {
        switch (_selectedOptionIndex)
        {
            case 0:
                OnResumeAction();
                break;
            case 1:
                OnSavePopupAction();
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

    private void OnSavePopupAction()
    {
        OpenSaveBox();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnSettingsAction()
    {
        Debug.Log("Settings");
    }
    private void ExecuteSelectedSaveAction()
    {
        switch (_selectedOptionIndexSave)
        {
            case 0:
                OnSaveAction();
                break;
            case 1:
                OnSaveAndExitAction();
                break;
            case 2:
                OnCancelAction();
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnSaveAction()
    {
        SaveGameData();
        CloseSaveBox();
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private void OnSaveAndExitAction()
    {
        SaveGameData();
        Application.Quit();
    }

    private void OnCancelAction()
    {
        CloseSaveBox();
    }

    #endregion

    #region UI Popup and Navigation

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
        _isDialogOpen = !_isDialogOpen;

        if (_isDialogOpen)
        {
            OpenDialogBox();
        }
        else
        {
            CloseDialogBox();
            if (_isSaveOpen)
            {
                CloseSaveBox();
            }
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
    
    #region Save Popup and Navigation

    private void HandleSaveNavigation(Vector2 uiInput)
    {
        if (_navigationProcessedThisFrame)
            return;

        if (uiInput.x < 0 && !_wasXPressed)
        {
            _selectedOptionIndexSave = (_selectedOptionIndexSave - 1 + 3) % 3;
            _navigationProcessedThisFrame = true;
            _wasXPressed = true;
        }
        else if (uiInput.x > 0 && !_wasXPressed)
        {
            _selectedOptionIndexSave = (_selectedOptionIndexSave + 1) % 3;
            _navigationProcessedThisFrame = true;
            _wasXPressed = true;
        }
        else if (uiInput.x == 0)
        {
            _wasXPressed = false;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateSaveUI()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform option = _currentSaveBox.transform.Find($"Canvas/Foreground/Options/Option{i}");
            if (option != null)
            {
                TextMeshProUGUI optionText = option.Find("Text")?.GetComponent<TextMeshProUGUI>();
                Transform fillImageTransform = option.Find("Fill");

                if (fillImageTransform != null)
                {
                    fillImageTransform.gameObject.SetActive(i == _selectedOptionIndexSave);
                }

                if (optionText != null)
                {
                    optionText.fontStyle = i == _selectedOptionIndexSave ? FontStyles.Bold : FontStyles.Normal;
                }
            }
        }
    }
    

    private void OpenSaveBox()
    {
        Destroy(_currentSaveBox);
        _currentSaveBox = Instantiate(savePopupPrefab, transform);
        _input.uiControls.Enable();
        _input.movement.Disable();
        _isSaveOpen = true;
    }

    private void CloseSaveBox()
    {
        if (_currentSaveBox != null)
        {
            Destroy(_currentSaveBox);
        }

        _isSaveOpen = false;
        _input.uiControls.Enable();
        _input.movement.Disable();
    }

    #endregion
    
    #region Save and Load UI State

    public void SaveUIState()
    {
        PlayerPrefs.SetString("ActivePanel_Scene" + gameObject.scene.buildIndex, "MainMenu");
        PlayerPrefs.Save();
    }

    public void LoadUIState()
    {
        string activePanel = PlayerPrefs.GetString("ActivePanel_Scene" + gameObject.scene.buildIndex, "MainMenu");
    }

    #endregion

    #region Save and Load Game Data

    public void SaveGameData()
    {
        SaveLoadSystem.SaveGameData(playerController, FindObjectOfType<PlotDataHandler>());
    }

    public void LoadGameData()
    {
        SaveLoadSystem.LoadGameData(playerController, FindObjectOfType<PlotDataHandler>());
    }

    #endregion
}
