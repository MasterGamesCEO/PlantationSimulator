using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    private Controls controls;

    public Vector2 Move
    {
        get;
        private set;
    }
    public Vector2 Look
    {
        get;
        private set;
    }

    public InputAction FireAction
    {
        get; private set;
    }

    public InputAction AimAction
    {
        get; private set;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // This exposes the entire event
        FireAction = controls.Locomotion.Shoot;
        AimAction = controls.Locomotion.ADS;
        
    }

    // Update is called once per frame
    void Update()
    {
        Move = controls.Locomotion.Move.ReadValue<Vector2>();
        Look = controls.Locomotion.Look.ReadValue<Vector2>();
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
        
        FireAction = controls.Locomotion.Shoot;
        AimAction = controls.Locomotion.ADS;
    }
}
