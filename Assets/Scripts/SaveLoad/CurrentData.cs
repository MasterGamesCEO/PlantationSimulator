using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaveLoad
{
    [System.Serializable]
    public class CurrentData : MonoBehaviour
    {
        public UiData uiData = new UiData();
        public GameplayData gameplayData = new GameplayData();
        public RobotData robotData = new RobotData();

        [System.Serializable]
        public class UiData
        {
            public float saveMoney;
            public float cropData;
        }
        [System.Serializable]
        public class GameplayData
        {
            public List<PlotStats> gameplayPlotStats;
            public List<PlatformStats> gameplayPlatformStats;
        }
        [System.Serializable]
        public class RobotData
        {
            public List<RobotAttributes> workingRobots;
            public List<RobotAttributes> unassignedRobots;
        }
   

        public void SaveFile()
        {
            if (FindObjectOfType<PlotDataHandler>())
            {
                SavePlotsToJson();
            }

            if (FindObjectOfType<PlotPricePopupScript>())
            {
                SaveMoneyToJson();
            }

            if (FindObjectOfType<RobotManager>())
            {
                SaveRobotsToJson();
            }
        }
        public void LoadFile()
        {
            if (FindObjectOfType<PlotDataHandler>())
            {
                LoadPlotsFromJson();
            }

            if (FindObjectOfType<UIManager>())
            {
                LoadMoneyFromJson();
            }
            if (FindObjectOfType<RobotManager>())
            {
                LoadRobotsFromJson();
            }
        }
    
        public void SaveMoneyToJson()
        {
            string filePath = Application.persistentDataPath + $"/SavedMoneyDataFor{_slotLastSelected}.json";
            Debug.Log(filePath);
        
            string currentData = JsonUtility.ToJson(uiData);
            Debug.Log(currentData);
            System.IO.File.WriteAllText(filePath,currentData);
        
            Debug.Log("Saved Money");
        }
        public void SavePlotsToJson()
        {
            string filePath = Application.persistentDataPath + $"/SavedPlotDataFor{_slotLastSelected}.json";
            Debug.Log(filePath);
        
            string currentData = JsonUtility.ToJson(gameplayData);
            Debug.Log(currentData);
            System.IO.File.WriteAllText(filePath,currentData);
        
            Debug.Log("Saved Plots");
        }
        public void SaveRobotsToJson()
        {
            string filePath = Application.persistentDataPath + $"/SavedRobotDataFor{_slotLastSelected}.json";
            Debug.Log(filePath);
        
            string currentData = JsonUtility.ToJson(robotData);
            Debug.Log(robotData);
            System.IO.File.WriteAllText(filePath,currentData);
        
            Debug.Log("Saved Robots");
        }
        public void LoadMoneyFromJson()
        {
            string filePath = Application.persistentDataPath + $"/SavedMoneyDataFor{_slotLastSelected}.json";
            string currentData = System.IO.File.ReadAllText(filePath);
            Debug.Log(currentData);
            uiData = JsonUtility.FromJson<UiData>(currentData);
            NumberCounter numberCounter = FindObjectOfType<NumberCounter>();
            numberCounter.Value = Instance.uiData.cropData;
            Debug.Log("Loaded Money");
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public void LoadPlotsFromJson()
        {
            string filePath = Application.persistentDataPath + $"/SavedPlotDataFor{_slotLastSelected}.json";

            if (System.IO.File.Exists(filePath))
            {
                string currentData = System.IO.File.ReadAllText(filePath);
                Debug.Log(currentData);

                try
                {
                    gameplayData = JsonUtility.FromJson<GameplayData>(currentData);

                    foreach (var x in gameplayData.gameplayPlotStats)
                    {
                        Debug.Log(x.ToString());
                    }
                    foreach (var x in gameplayData.gameplayPlatformStats)
                    {
                        Debug.Log(x.ToString());
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error deserializing JSON: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"File not found at path: {filePath}");
            }
        }
        public void LoadRobotsFromJson()
        {
            string filePath = Application.persistentDataPath + $"/SavedRobotDataFor{_slotLastSelected}.json";
            string currentData = System.IO.File.ReadAllText(filePath);
            Debug.Log(currentData);
            robotData = JsonUtility.FromJson<RobotData>(currentData);
            Debug.Log("Loaded Robots");
        }


        public void Reset()
        {
            PlotDataHandler plotDataHandler = FindObjectOfType<PlotDataHandler>();
            PlatformDataHandler platformDataHandler = FindObjectOfType<PlatformDataHandler>();
            UiData newUiData = new UiData
            {
                saveMoney = 10000f,
                cropData = 0f
            };
            GameplayData newGameplayData = new GameplayData()
            {
                gameplayPlotStats = plotDataHandler.ResetPlotData(),
                gameplayPlatformStats = platformDataHandler.ResetPlatformData()
            };
        
            string currentMoneyData = JsonUtility.ToJson(newUiData);
            string currentPlotData = JsonUtility.ToJson(newGameplayData);
            string filePathMoney = Application.persistentDataPath + $"/SavedMoneyDataFor{_slotLastSelected}.json";
            string filePathPlots = Application.persistentDataPath + $"/SavedPlotDataFor{_slotLastSelected}.json";
            System.IO.File.WriteAllText(filePathMoney,currentMoneyData);
            System.IO.File.WriteAllText(filePathPlots,currentPlotData);
            Debug.Log("Reset json");
            LoadFile();
        }
    
        #region Singleton Data
        private int _slotLastSelected;
        public int SlotLastSelectedData
        {
            get => _slotLastSelected;
            set => _slotLastSelected = value;
        }
    
        private static CurrentData _instance;
        public static CurrentData Instance
        {
            get
            {
                if (_instance == null)
                {

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CurrentData");
                        _instance = go.AddComponent<CurrentData>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        #endregion
    
    }
}