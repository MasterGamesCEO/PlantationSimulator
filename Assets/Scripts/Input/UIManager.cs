using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private GameObject savePopupPrefab;
    [SerializeField] private GameObject dialogBoxPrefab;
    [SerializeField] private PlayerController playerController;
    [FormerlySerializedAs("_plotPricePopupScript")] [SerializeField] private PlotPricePopupScript plotPricePopupScript;

    private InputManager _input;
    private GameObject _currentSaveBox;
    private GameObject _currentDialogBox;
    private SaveData _saveData;
    
    private int _selectedOptionIndex = 0;
    private int _selectedOptionIndexSave = 0;
    [FormerlySerializedAs("_isDialogOpen")] public bool isDialogOpen = false;
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
            plotPricePopupScript.UpdateMoney(0);
        }
    }

    private void Start()
    {
        _input = InputManager.Instance;
        _saveData = SaveData.Instance;
        _input.openDialog.performed += OnDialogPerformed;
        
        LoadGameData();
    }

    private void Update()
    {
        if (isDialogOpen && !_isSaveOpen)
        {
            Vector2 uiInput = _input.uiControls.ReadValue<Vector2>();
            HandleNavigation(uiInput);
            if (_input.enterKey.triggered)
            {
                ExecuteSelectedAction();
            }
            UpdateUI();
        }
        else if(isDialogOpen && _isSaveOpen)
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
                OnSaveAction();
                break;
            case 2:
                OnOptionsAction();
                break;
            case 3:
                OnMenuAction();
                break;
            case 4:
                OnSaveOpen();
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
        CloseDialogBox();
    }
    private void OnOptionsAction()
    {
        Debug.Log("Options");
        CurrentData.Instance.LoadFile();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnMenuAction()
    {
        SaveGameData();
        CloseSaveBox();
        SceneController sceneController = FindObjectOfType<SceneController>();
        sceneController.ChangeScene(0);
    }
    private void OnSaveOpen()
    {
        OpenSaveBox();
    }
    
    
    private void ExecuteSelectedSaveAction()
    {
        switch (_selectedOptionIndexSave)
        {
            case 0:
                OnCancelAction();
                break;
            case 1:
                OnSaveAndExitAction();
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
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
            _selectedOptionIndex = (_selectedOptionIndex - 1 + 5) % 5;
            _navigationProcessedThisFrame = true;
            _wasYPressed = true;
        }
        else if (uiInput.y < 0 && !_wasYPressed)
        {
            _selectedOptionIndex = (_selectedOptionIndex + 1) % 5;
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
        for (int i = 0; i < 5; i++)
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
        isDialogOpen = !isDialogOpen;

        if (isDialogOpen)
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

        isDialogOpen = false;
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
            _selectedOptionIndexSave = (_selectedOptionIndexSave - 1 + 2) % 2;
            _navigationProcessedThisFrame = true;
            _wasXPressed = true;
        }
        else if (uiInput.x > 0 && !_wasXPressed)
        {
            _selectedOptionIndexSave = (_selectedOptionIndexSave + 1) % 2;
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
        for (int i = 0; i < 2; i++)
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
        _input.uiControls.Disable();
        _input.movement.Enable();
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
        // Make sure _saveData is not null before calling methods on it
        if (_saveData != null)
        {
            _saveData.SaveGameData(_saveData.SlotLastSelectedData);
        }
        else
        {
            Debug.LogWarning("SaveData instance not found.");
        }
    }

    public void LoadGameData()
    {
        // Make sure _saveData is not null before calling methods on it
        if (_saveData != null)
        {
            _saveData.LoadGameData(_saveData.SlotLastSelectedData);
        }
        else
        {
            Debug.LogWarning("SaveData instance not found.");
        }
    }

    #endregion
}
