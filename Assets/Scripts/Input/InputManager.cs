using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    private Controls controls;
    private PlayerController _playerController;
    public InputAction buyLand;
    
    public Vector2 Move
    {
        get;
        private set;
    }
    
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        
        controls = new Controls();
        controls.Enable();
        buyLand = controls.Dialog.BuyLand;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move = controls.Player.Movement.ReadValue<Vector2>();
    }

}
