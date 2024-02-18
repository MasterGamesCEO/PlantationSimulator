using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private Controls _controls;

    public InputAction buyLand;
    public InputAction uiControls;
    public InputAction movement; 
    public InputAction openDialog;

    public Vector2 Move
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _controls = new Controls();
        _controls.Enable();

        buyLand = _controls.Dialog.BuyLand;
        uiControls = _controls.Dialog.uiControls;
        movement = _controls.Player.Movement;
        openDialog = _controls.Dialog.openDialog;
    }

    private void OnEnable()
    {
        buyLand.Enable();
        uiControls.Enable();
        movement.Enable();  
        openDialog.Enable();
    }

    private void OnDisable()
    {
        buyLand.Disable();
        uiControls.Disable();
        movement.Disable();  
        openDialog.Disable();
    }

    private void Update()
    {
        Move = movement.ReadValue<Vector2>();  
        
    }
}