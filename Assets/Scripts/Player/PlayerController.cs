using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;



public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private InputManager input;

    private UiScript _uiScript;

    [SerializeField]
    private float Speed = 5;

    private PlotStats curPlotStats;
    
    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.instance;
        
        controller = GetComponent<CharacterController>();
        _uiScript = gameObject.GetComponent<UiScript>();
        
        input.buyLand.performed += OnBuyLandPerformed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement(Time.deltaTime);
    }

    private void HandleMovement(float delta)
    {
        //multiplying by transform.right and transform.forward will make all movement relative to camera direction. 
        Vector3 movement = (input.Move.x * transform.right) + (input.Move.y * transform.forward);
        
        controller.Move(movement * (Speed * delta));
    }

    
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag.Equals("Plot"))
        {
            curPlotStats = other.gameObject.GetComponent<PlotStats>();
            if (curPlotStats.isLocked == true)
            {
                Debug.Log("LOCKED PLOT");
                _uiScript.activatePopup();
            }
            else
            {
                Debug.Log("UNLOCKED PLOT");
                curPlotStats.deactivateBoundry();
                _uiScript.deactivatePopup();
            }
            
        }
        else if (other.gameObject.tag.Equals("Onplot"))
        {
            _uiScript.deactivatePopup();
        }
    }

    private void OnBuyLandPerformed(InputAction.CallbackContext obj)
    {
        if (_uiScript.popupActive())
        {
            
            _uiScript.deactivatePopup();
            curPlotStats.isLocked = false;
            curPlotStats.deactivateBoundry();
            _uiScript.updateMoney("0");
        }
        
    }
    
}
