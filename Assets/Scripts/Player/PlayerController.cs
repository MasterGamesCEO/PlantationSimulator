using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private InputManager _input;
    private PlotStats _curPlotStats;

    [SerializeField] private PlotPricePopupScript _plotPricePopupScript;
    [SerializeField] private float speed = 5;
    [SerializeField] public float playerMoney = 200;

    public float GetPlayerMoney()
    {
        return playerMoney;
    }

    void Start()
    {
        _input = InputManager.Instance;
        _controller = GetComponent<CharacterController>();
        _input.buyLand.performed += OnBuyLandPerformed;
    }

    void FixedUpdate()
    {
        HandleMovement(Time.deltaTime);
    }

    private void HandleMovement(float delta)
    {
        Vector3 movement = (_input.Move.x * transform.right) + (_input.Move.y * transform.forward);
        _controller.Move(movement * (speed * delta));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Plot"))
        {
            _curPlotStats = other.gameObject.GetComponent<PlotStats>();
            if (_curPlotStats.isLocked == true)
            {
                Debug.Log("LOCKED PLOT");
                _plotPricePopupScript.ActivatePopup(_curPlotStats.PlotPrice);
            }
            else
            {
                Debug.Log("UNLOCKED PLOT");
                _curPlotStats.DeactivateBoundry();
                _plotPricePopupScript.DeactivatePopup();
            }
            
        } else if (other.gameObject.tag.Equals("House"))
        {
            // Call a method to switch to the market scene
            SwitchToMarketScene();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Plot"))
        {
            _plotPricePopupScript.DeactivatePopup();
        }
    }
    

    private void OnBuyLandPerformed(InputAction.CallbackContext obj)
    {
        if (_plotPricePopupScript.PopupActive())
        {
            if (playerMoney >= _curPlotStats.PlotPrice)
            {
                _plotPricePopupScript.DeactivatePopup();
                _curPlotStats.isLocked = false;
                _curPlotStats.DeactivateBoundry();
                _plotPricePopupScript.UpdateMoney(_curPlotStats.PlotPrice);
                playerMoney = playerMoney - _curPlotStats.PlotPrice;
            }
            else
            {
                Debug.Log("Too broke lil homie");
            }
        }
        else
        {
            Debug.Log("No Plot ready to buy");
        }
    }
    private void SwitchToMarketScene()
    {
        SceneManager.LoadScene(1); 
    }

    
}