using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBoxPrefab; // Prefab of your dialog box
    private GameObject currentDialogBox;

    [SerializeField] private PlayerController playerController; // Reference to the PlayerController script

    private bool isDialogOpen = false;
    private InputManager _input;

    private void Start()
    {
        
        // Get a reference to the uiControls action
        _input = InputManager.Instance;

        // Subscribe to UIControls action
        _input.openDialog.performed += OnDialogPerformed;
    }

    private void Update()
    {
        if (isDialogOpen)
        {
            // Read input only if the dialog is open
            Vector2 uiInput = _input.uiControls.ReadValue<Vector2>();
            // Handle navigation logic based on input inside the dialog
            // Update UI elements accordingly
            // You might want to create a method in the dialog box script to handle this logic
        }
    }

    private void OnDialogPerformed(InputAction.CallbackContext obj)
    {
        if (isDialogOpen)
        {
            CloseDialogBox();
        }
        else
        {
            OpenDialogBox();
        }
    }

    private void OpenDialogBox()
    {
        // Destroy the current dialog box if it exists
        Debug.Log("Opened Dialog");
        Destroy(currentDialogBox);

        // Instantiate the dialog box prefab
        currentDialogBox = Instantiate(dialogBoxPrefab, transform);

        // Set isDialogOpen to true
        isDialogOpen = true;

        // Set input focus to uiControls action when the dialog is open
        _input.uiControls.Enable();
    }

    private void CloseDialogBox()
    {
        if (currentDialogBox != null)
        {
            // Destroy the current dialog box
            Destroy(currentDialogBox);
        }

        // Set isDialogOpen to false
        isDialogOpen = false;

        // Disable input focus on uiControls action when the dialog is closed
        _input.uiControls.Disable();
    }
}
