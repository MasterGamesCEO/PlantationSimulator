using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Fields

    private CharacterController _controller;
    private InputManager _input;
    private PlotStats _curPlotStats;

    [SerializeField] private PlotPricePopupScript _plotPricePopupScript;
    [SerializeField] private float speed = 5;
    [SerializeField] private float playerMoney = 10000;

    #endregion

    #region Properties

    public float GetPlayerMoney()
    {
        return playerMoney;
    }

    public void SetPlayerMoney(float amount)
    {
        playerMoney = amount;
    }

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        _input = InputManager.Instance;
        _controller = GetComponent<CharacterController>();
        _input.buyLand.performed += OnBuyLandPerformed;
    }

    private void FixedUpdate()
    {
        HandleMovement(Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Plot"))
        {
            HandlePlotTrigger(other);
        }
        else if (other.gameObject.tag.Equals("House"))
        {
            SwitchScenes();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Plot"))
        {
            _plotPricePopupScript.DeactivatePopup();
        }
    }

    #endregion

    #region Player Actions

    private void HandleMovement(float delta)
    {
        Vector3 movement = (_input.Move.x * transform.right) + (_input.Move.y * transform.forward);
        _controller.Move(movement * (speed * delta));
    }

    private void OnBuyLandPerformed(InputAction.CallbackContext obj)
    {
        if (_plotPricePopupScript.PopupActive())
        {
            BuyLand();
        }
        else
        {
            Debug.Log("No Plot ready to buy");
        }
    }

    private void HandlePlotTrigger(Collider plotCollider)
    {
        _curPlotStats = plotCollider.gameObject.GetComponent<PlotStats>();
        if (_curPlotStats.isLocked)
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
    }

    private void BuyLand()
    {
        if (playerMoney >= _curPlotStats.PlotPrice)
        {
            _plotPricePopupScript.DeactivatePopup();
            _curPlotStats.isLocked = false;
            _curPlotStats.DeactivateBoundry();
            _plotPricePopupScript.UpdateMoney(_curPlotStats.PlotPrice);
            playerMoney -= _curPlotStats.PlotPrice;
            _plotPricePopupScript.RunMoneySpread();
        }
        else
        {
            Debug.Log("Too broke lil homie");
        }
    }

    #endregion

    #region Scene Transition

    private void SwitchScenes()
    {
        SaveLoadSystem.SaveGameData(this, FindObjectOfType<PlotDataHandler>());

        SceneController currentScene = FindObjectOfType<SceneController>();
        currentScene.ChangeScene();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveLoadSystem.LoadGameData(this, FindObjectOfType<PlotDataHandler>());

        
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.LoadUIState();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion
}
