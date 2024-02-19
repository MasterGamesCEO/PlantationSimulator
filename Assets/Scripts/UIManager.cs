using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject dialogBoxPrefab;
    [SerializeField] private PlayerController playerController;
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
        PlayerPrefs.SetFloat("PlayerMoney", playerController.GetPlayerMoney());
        var position = playerController.transform.position;
        PlayerPrefs.SetFloat("PlayerPositionX", position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", position.z);

        PlotDataHandler plotDataHandler = FindObjectOfType<PlotDataHandler>();
        if (plotDataHandler != null)
        {
            plotDataHandler.SavePlotData();
        }

        PlayerPrefs.Save();
    }

    public void LoadGameData()
    {
        playerController.SetPlayerMoney(PlayerPrefs.GetFloat("PlayerMoney", 10000f));
        _plotPricePopupScript.UpdateMoney(0);

        float playerPositionX = PlayerPrefs.GetFloat("PlayerPositionX", DefaultPlayerPositionX);
        float playerPositionY = PlayerPrefs.GetFloat("PlayerPositionY", DefaultPlayerPositionY);
        float playerPositionZ = PlayerPrefs.GetFloat("PlayerPositionZ", DefaultPlayerPositionZ);

        var transform1 = playerController.transform;
        transform1.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

        PlotDataHandler plotDataHandler = FindObjectOfType<PlotDataHandler>();
        if (plotDataHandler != null)
        {
            plotDataHandler.LoadPlotData();
        }
    }
    
    private void ResetGameData()
    {
        playerController.SetPlayerMoney(10000);
        _plotPricePopupScript.UpdateMoney(0);

        PlotDataHandler plotDataHandler = FindObjectOfType<PlotDataHandler>();

        if (plotDataHandler != null)
        {
            plotDataHandler.ResetGameData();
        }
        
        PlayerPrefs.DeleteAll();
        playerController.transform.position = new Vector3(DefaultPlayerPositionX, DefaultPlayerPositionY, DefaultPlayerPositionZ);
    }

    #endregion
}
