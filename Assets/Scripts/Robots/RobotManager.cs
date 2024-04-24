using System;
using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class RobotManager : MonoBehaviour
{
    // Robot Prefabs
    [Header("Auction prefab")]
    [SerializeField] public GameObject basicScrawnyACPrefab;
    [SerializeField] public GameObject basicNormalACPrefab;
    [SerializeField] public GameObject silverNormalACPrefab;
    [SerializeField] public GameObject silverTallACPrefab;
    [SerializeField] public GameObject goldTallACPrefab;
    [SerializeField] public GameObject goldBuiltACPrefab;
    [SerializeField] public GameObject diamondBuiltACPrefab;
    [SerializeField] public GameObject diamondMassiveACPrefab;
    [SerializeField] public GameObject ultraMassiveACPrefab;
    [SerializeField] public GameObject ultraTankACPrefab;
    [Header("Plot prefab")]
    [SerializeField] public GameObject basicScrawnyPrefab;
    [SerializeField] public GameObject basicNormalPrefab;
    [SerializeField] public GameObject silverNormalPrefab;
    [SerializeField] public GameObject silverTallPrefab;
    [SerializeField] public GameObject goldTallPrefab;
    [SerializeField] public GameObject goldBuiltPrefab;
    [SerializeField] public GameObject diamondBuiltPrefab;
    [SerializeField] public GameObject diamondMassivePrefab;
    [SerializeField] public GameObject ultraMassivePrefab;
    [SerializeField] public GameObject ultraTankPrefab;
    
    [SerializeField] public List<RobotAttributes> workingRobots;
    [SerializeField] public List<RobotAttributes> unassignedRobots;
    [SerializeField] public List<RobotAttributes> auctionRobots;

    private SaveData _saveData;

    public void SaveRobotData()
    {
        List<RobotAttributes> saveWorkingRobotAttributesList = new List<RobotAttributes>();
        List<RobotAttributes> saveUnassignedRobotAttributesList = new List<RobotAttributes>();
        for (int i = 0; i < workingRobots.Count; i++)
        {
            saveWorkingRobotAttributesList.Add(workingRobots[i]);
            Debug.Log("CPS For " + i + " " + workingRobots[i].cps);
        }
        for (int i = 0; i < unassignedRobots.Count; i++)
        {
            saveUnassignedRobotAttributesList.Add(unassignedRobots[i]);
            Debug.Log("CPS For " + i + " " + unassignedRobots[i].cps);
        }
        CurrentData.Instance.robotData.workingRobots = saveWorkingRobotAttributesList;
        CurrentData.Instance.robotData.unassignedRobots = saveUnassignedRobotAttributesList;
    }

    public void LoadRobotData()
    {
        workingRobots = CurrentData.Instance.robotData.workingRobots;
        unassignedRobots = CurrentData.Instance.robotData.unassignedRobots;
    }
    private void Start()
    {
        _saveData = SaveData.Instance;
    }
    
    public void BuyRobot(RobotAttributes attributes)
    {
        if (unassignedRobots.Count < 4 && CurrentData.Instance.uiData.saveMoney >= attributes.price)
        {
            unassignedRobots.Add(attributes);
            _saveData.playerMoney -= attributes.price;
            LoadSave(_saveData.SlotLastSelectedData);
        }
        else
        {
            Debug.Log("Maximum number of available robots reached or too broke");
        }
        
    }
    public void LoadSave(int slotIndex)
    {
        _saveData.SaveGameData(_saveData.SlotLastSelectedData);
        if (_saveData.DoesSaveExist(slotIndex))
        {
            SceneController sceneController = FindObjectOfType<SceneController>();
            StartCoroutine(LoadSceneAndData(slotIndex, sceneController));
        }
        else
        {
            Debug.Log("No save data found for Slot " + slotIndex);
        }
    }
    private IEnumerator LoadSceneAndData(int slotIndex, SceneController sceneController)
    {
        // Start the scene transition
        _saveData.SlotLastSelectedData = slotIndex;
        CurrentData.Instance.SlotLastSelectedData = slotIndex;
        sceneController.ChangeScene(2);

        // Wait for the scene transition to complete
        while (sceneController.currentlySceneChanging)
        {
            yield return null;
        }

        // Scene transition is complete, now load the data
        _saveData.Load(slotIndex);
    }
}
