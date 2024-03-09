using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Singleton

    public static InputManager Instance { get; private set; }

    #endregion

    #region Fields

    private Controls _controls;

    public InputAction buyLand;
    public InputAction uiControls;
    public InputAction movement;
    public InputAction openDialog;
    public InputAction enterKey;
    public InputAction backKey;
    public InputAction sprint;
    public InputAction assignRobot;
    public Vector2 Move { get; private set; }

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        Instance = this;
        _controls = new Controls();
        _controls.Enable();

        InitializeInputActions();
        DisableUIControls();
    }

    private void OnEnable()
    {
        EnableInputActions();
    }

    private void OnDisable()
    {
        DisableInputActions();
    }

    private void Update()
    {
        Move = movement.ReadValue<Vector2>();
        
    }

    #endregion

    #region Input Actions

    private void InitializeInputActions()
    {
        buyLand = _controls.Dialog.BuyLand;
        uiControls = _controls.Dialog.uiControls;
        movement = _controls.Player.Movement;
        openDialog = _controls.Dialog.openDialog;
        enterKey = _controls.Dialog.enterKey;
        backKey = _controls.Dialog.BackKey;
        sprint = _controls.Player.Sprint;
        assignRobot = _controls.Dialog.AssignRobot;
    }

    private void EnableInputActions()
    {
        buyLand.Enable();
        movement.Enable();
        openDialog.Enable();
    }

    private void DisableInputActions()
    {
        buyLand.Disable();
        movement.Disable();
        openDialog.Disable();
    }

    #endregion

    #region UI Controls

    private void DisableUIControls()
    {
        uiControls.Disable();
    }

    #endregion
}