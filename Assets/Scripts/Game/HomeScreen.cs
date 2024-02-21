using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] private GameObject startOptionsPrefab;
    [SerializeField] private GameObject slotPrefab;

    private InputManager _input;

    private GameObject _currentStart;
    private GameObject _currentSlot;

    private int _selectedOptionIndexStart = 1;
    private int _selectedOptionIndexSlot = 0;
    private int _selectedOptionIndexVerticalSlot = 0;

    private bool _isStartOpen = true;
    private bool _isSlotsOpen = false;

    private bool _navigationProcessedThisFrame = false;
    private bool _wasYPressed = false;
    private bool _wasXPressed = false;

    private SaveData _saveData; // Reference to the SaveData instance


    private void Start()
    {
        _input = InputManager.Instance;
        _input.openDialog.performed += OnDialogPerformed;
        OpenStartBox();
        _saveData = SaveData.Instance;
    }

    private void Update()
    {
        Vector2 uiInput = _input.uiControls.ReadValue<Vector2>();

        if (_isStartOpen)
        {
            HandleStartNavigation(uiInput);
            if (_input.enterKey.triggered)
            {
                ExecuteStartSelectedAction();
            }

            UpdateStartUI();
        }
        else if (_isSlotsOpen)
        {
            HandleSlotNavigation(uiInput);
            if (_input.enterKey.triggered)
            {
                ExecuteSlotSelectedAction();
            }

            UpdateSlotUI();
        }

        _navigationProcessedThisFrame = false;
    }

    private void OnDialogPerformed(InputAction.CallbackContext obj)
    {
        if (_isStartOpen)
        {
            OpenStartBox();
        }
    }

    #region UI Navigation

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

    private void HandleSlotNavigation(Vector2 uiInput)
    {
        if (_navigationProcessedThisFrame)
            return;

        if (uiInput.x < 0 && !_wasXPressed)
        {
            _selectedOptionIndexSlot = (_selectedOptionIndexSlot - 1 + 3) % 3;
            _navigationProcessedThisFrame = true;
            _wasXPressed = true;
        }
        else if (uiInput.x > 0 && !_wasXPressed)
        {
            _selectedOptionIndexSlot = (_selectedOptionIndexSlot + 1) % 3;
            _navigationProcessedThisFrame = true;
            _wasXPressed = true;
        }
        else if (uiInput.x == 0)
        {
            _wasXPressed = false;
        }

        if (uiInput.y < 0 && !_wasYPressed)
        {
            _selectedOptionIndexVerticalSlot = (_selectedOptionIndexVerticalSlot - 1 + 2) % 2;
            _navigationProcessedThisFrame = true;
            _wasYPressed = true;
        }
        else if (uiInput.y > 0 && !_wasYPressed)
        {
            _selectedOptionIndexVerticalSlot = (_selectedOptionIndexVerticalSlot + 1) % 2;
            _navigationProcessedThisFrame = true;
            _wasYPressed = true;
        }
        else if (uiInput.y == 0)
        {
            _wasYPressed = false;
        }
    }

    #endregion

    #region UI Updates


    private void UpdateStartUI()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform option = _currentStart.transform.Find($"Canvas/Foreground/Options/Option{i}");
            if (option != null)
            {
                TextMeshProUGUI optionText = option.Find("Text")?.GetComponent<TextMeshProUGUI>();
                Transform fillImageTransform = option.Find("Fill");

                if (fillImageTransform != null)
                {
                    fillImageTransform.gameObject.SetActive(i == _selectedOptionIndexStart);
                }

                if (optionText != null)
                {
                    optionText.fontStyle = i == _selectedOptionIndexStart ? FontStyles.Bold : FontStyles.Normal;
                }
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateSlotUI()
    {
        for (int f = 0; f < 3; f++)
        {
            for (int i = 0; i < 2; i++)
            {
                Transform option = _currentSlot.transform.Find($"Canvas/Foreground/Slots/Slot{f}/Option{i}");
                if (option != null)
                {
                    TextMeshProUGUI optionText = option.Find("Text")?.GetComponent<TextMeshProUGUI>();
                    Transform fillImageTransform = option.Find("Fill");

                    if (fillImageTransform != null)
                    {
                        fillImageTransform.gameObject.SetActive(f == _selectedOptionIndexSlot &&
                                                                i == _selectedOptionIndexVerticalSlot);
                    }

                    if (optionText != null)
                    {
                        if (f == _selectedOptionIndexSlot && i == _selectedOptionIndexVerticalSlot)
                        {
                            optionText.fontStyle = FontStyles.Bold;
                        }
                        else
                        {
                            optionText.fontStyle = FontStyles.Normal;
                        }
                    }
                }
            }
        }
    }


    #endregion

    #region Execute Actions

    private void ExecuteStartSelectedAction()
    {
        if (_selectedOptionIndexStart == 1)
        {
            OpenSlotBox();

        }
        // Add other actions for start options as needed
    }

    private void ExecuteSlotSelectedAction()
    {
        // Add actions for slot options based on _selectedOptionIndexSlot
        // For example, load or create a new save, reset, etc.
        if (_selectedOptionIndexVerticalSlot == 0)
        {
            if (_saveData.DoesSaveExist(_selectedOptionIndexSlot))
            {
                Debug.Log("Loading Save");
                LoadSave(_selectedOptionIndexSlot);
            }
            else
            {
                // If no save exists, create a new save
                CreateNewSave(_selectedOptionIndexSlot);
            }
        }
        else if (_selectedOptionIndexVerticalSlot == 1)
        {
            ResetSave(_selectedOptionIndexSlot);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void LoadSave(int slotIndex)
    {
        if (_saveData.DoesSaveExist(slotIndex))
        {
            SceneManager.LoadScene(0);
            
            _saveData.SlotLastSelectedData = slotIndex;
            _saveData.Load(slotIndex);


        }
        else
        {
            Debug.Log("No save data found for Slot " + slotIndex);
        }
    }

    public void ResetSave(int slotIndex)
    {
        if (slotIndex == 0)
        {
            _saveData._ifPlotReset0 = true;
        }

        if (slotIndex == 1)
        {
            _saveData._ifPlotReset1 = true;
        }

        if (slotIndex == 2)
        {
            _saveData._ifPlotReset2 = true;
        }

        Debug.Log("Resetting save data for Slot " + slotIndex);
    }
    

    #endregion

    private void OpenStartBox()
    {
        if (_currentStart != null)
        {
            Destroy(_currentStart);
        }

        _currentStart = Instantiate(startOptionsPrefab, transform);
        _input.uiControls.Enable();
        _input.movement.Disable();
        _isStartOpen = true;
        _isSlotsOpen = false;
    }

    private void OpenSlotBox()
    {
        Destroy(_currentSlot);
        _currentSlot = Instantiate(slotPrefab, transform);
        _input.uiControls.Enable();
        _input.movement.Disable();
        _isSlotsOpen = true;
        _isStartOpen = false;
    }

    // Implement the CreateNewSave and DeleteSave methods from ISaveSlotHandler
    // ReSharper disable Unity.PerformanceAnalysis
    public void CreateNewSave(int slotIndex)
    {
        // Check if a save already exists for the specified slotIndex
        if (_saveData.DoesSaveExist(slotIndex))
        {
            Debug.Log($"A save already exists for Slot {slotIndex}. If you want to overwrite, implement logic here.");
        }
        else
        {
            _saveData.SlotLastSelectedData = slotIndex;

            // Reset and get the updated SaveData instance
            SaveData newData = _saveData.Reset(slotIndex);
            
            // Optionally, you can load the new save immediately if needed
            LoadSave(slotIndex);
        }
    }
}
