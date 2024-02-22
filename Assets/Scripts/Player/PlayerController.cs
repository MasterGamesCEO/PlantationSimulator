using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    #region Fields

    private CharacterController _controller;
    private InputManager _input;
    private PlotStats _curPlotStats;

    [FormerlySerializedAs("_plotPricePopupScript")] [SerializeField] private PlotPricePopupScript plotPricePopupScript;
    [SerializeField] private float speed = 5;
    [SerializeField] private float playerMoney;
    
    private SaveData _saveData;
    
    #endregion

    #region Properties

    public float GetPlayerMoney()
    {
        return SaveData.Instance.PlayerMoneySave;
    }

    public void SetPlayerMoney(float amount)
    {
        playerMoney = amount;
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
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Plot"))
        {
            plotPricePopupScript.DeactivatePopup();
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
        _curPlotStats = plotCollider.gameObject.GetComponent<PlotStats>();
        if (_curPlotStats.isLocked)
        {
            Debug.Log("LOCKED PLOT");
            plotPricePopupScript.ActivatePopup(_curPlotStats.PlotPrice);
        }
        else
        {
            Debug.Log("UNLOCKED PLOT");
            _curPlotStats.DeactivateBoundry();
            plotPricePopupScript.DeactivatePopup();
        }
    }

    private void BuyLand()
    {
        if (playerMoney >= _curPlotStats.PlotPrice)
        {
            plotPricePopupScript.DeactivatePopup();
            _curPlotStats.isLocked = false;
            _curPlotStats.DeactivateBoundry();
            plotPricePopupScript.UpdateMoney(_curPlotStats.PlotPrice);
            playerMoney -= _curPlotStats.PlotPrice;
            SaveData.Instance.playerMoney = playerMoney;
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
