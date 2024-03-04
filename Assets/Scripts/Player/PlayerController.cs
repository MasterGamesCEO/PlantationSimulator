using System.Collections;
using SaveLoad;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    #region Fields

    private CharacterController _controller;
    private InputManager _input;
    private Plot _curPlot;
    private bool _isSprinting;

    [SerializeField] private PlotPricePopupScript plotPricePopupScript;
    [SerializeField] private RobotPopup robotPopupScript;
    [SerializeField] private float speed = 5;
    [SerializeField] private float playerMoney;
    [SerializeField] private Animator playerAnimation;
    
    private SaveData _saveData;
    public CurrentData currentData;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsSprinting = Animator.StringToHash("IsSprinting");

    #endregion

    #region Properties

    public void SetPlayerMoney(float amount)
    {
        playerMoney = amount;
        CurrentData.Instance.SaveMoney = playerMoney;
        SaveData.Instance.playerMoney = playerMoney;
    }

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        _input = InputManager.Instance;
        _saveData = SaveData.Instance;
        _controller = GetComponent<CharacterController>();
        _input.buyLand.performed += OnBuyLandPerformed;
        _input.sprint.performed += OnSprintPerformed;
        _input.sprint.canceled += OnSprintPerformed;
    }

    private void OnSprintPerformed(InputAction.CallbackContext obj)
    {
        if (_input.sprint.triggered)
        {
            _isSprinting = true;
        }
        else
        {
            _isSprinting = false;
        }
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
            Debug.Log("Current player money " + playerMoney);
            
            SwitchHomeScenes();
            
        } else if (other.gameObject.tag.Equals("Platform"))
        {
            HandlePlatformTrigger(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Plot"))
        {
            plotPricePopupScript.DeactivatePopup();
        } else if (other.gameObject.tag.Equals("Platform"))
        {
            robotPopupScript.DeactivatePopup();
        }
    }

    #endregion

    #region Player Actions

    
    private void HandleMovement(float delta)
    {
        Vector3 movement = new Vector3(_input.Move.x, 0, _input.Move.y);  // Ignore the y-axis for rotation
        bool isWalking = movement.magnitude > 0.1f;
        if (isWalking && !_isSprinting)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * delta);
            _controller.Move(transform.forward * (speed * delta));  
        }
        else if (isWalking && _isSprinting)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * delta);
            _controller.Move(transform.forward * ((speed+3) * delta));  
        }
        
        playerAnimation.SetBool(IsWalking, isWalking);
        playerAnimation.SetBool(IsSprinting, _isSprinting);
    }

    private void OnBuyLandPerformed(InputAction.CallbackContext obj)
    {
        if (plotPricePopupScript.PopupActive())
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
        _curPlot = plotCollider.gameObject.GetComponent<Plot>();
        if (_curPlot.isLocked)
        {
            Debug.Log("LOCKED PLOT");
            plotPricePopupScript.ActivatePopup(_curPlot.PlotPrice);
        }
        else
        {
            Debug.Log("UNLOCKED PLOT");
            _curPlot.DeactivateBoundary();
            plotPricePopupScript.DeactivatePopup();
        }
    }
    private void HandlePlatformTrigger(Collider plotCollider)
    {
        /*
        //TODO: add current platform robot stats
        //TODO: Here -> _curPlatformStats = plotCollider.gameObject.GetComponent<PlotStats>();
        if (//TODO: Current Platform has robot... _curPlatformStats.isEmpty)
        {
            Debug.Log("Empty Platform");
            robotPopupScript.ActivatePopup(); //TODO: Add currently unassigned robots to the popup;
        }
        else //Current platform has a robot
        {
            Debug.Log("Robot here");
            //TODO: Activate a un assign robot popup
        }
        */
        robotPopupScript.ActivatePopup();
    }

    private void BuyLand()
    {
        if (playerMoney >= _curPlot.PlotPrice)
        {
            plotPricePopupScript.DeactivatePopup();
            _curPlot.isLocked = false;
            _curPlot.DeactivateBoundary();
            plotPricePopupScript.UpdateMoney(_curPlot.PlotPrice);
            playerMoney -= _curPlot.PlotPrice;
            CurrentData.Instance.SaveMoney = playerMoney;
            plotPricePopupScript.RunMoneySpread();
        }
        else
        {
            Debug.Log("Too broke lil homie");
        }
    }

    #endregion

    #region Scene Transition

    private void SwitchHomeScenes()
    {
        SaveData.Instance.SaveGameData(_saveData.SlotLastSelectedData);
        SceneController currentScene = FindObjectOfType<SceneController>();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(LoadSceneAndData(_saveData.SlotLastSelectedData, currentScene, 2));
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            StartCoroutine(LoadSceneAndData(_saveData.SlotLastSelectedData, currentScene, 1));
        } 
        
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator LoadSceneAndData(int slotIndex, SceneController sceneController, int scene)
    {
        _saveData.SlotLastSelectedData = slotIndex;
        sceneController.ChangeScene(scene);
        
        while (sceneController.currentlySceneChanging)
        {
            yield return null;
        }
        _saveData.Load(slotIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveData.Instance.LoadGameData(_saveData.SlotLastSelectedData);

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.LoadUIState();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion
}
