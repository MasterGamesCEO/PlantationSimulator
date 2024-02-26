using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlotDataHandler : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] public PlotStats[] allPlots;
    
    public List<SaveData.PlotData> GetPlotDataList()
    {
        List<SaveData.PlotData> plotDataList = new List<SaveData.PlotData>();
        foreach (var plot in allPlots)
        {
            plotDataList.Add(new SaveData.PlotData(plot.isLocked));
        }
        return plotDataList;
    }

    #region Save and Load Plot Data

    public void SavePlotData(int slotIndex)
    {
        
        for (int i = 0; i < allPlots.Length; i++)
        {
            PlayerPrefs.SetInt($"Plot_{slotIndex}_{i}_IsLocked", allPlots[i].isLocked ? 1 : 0);
        }
        
        PlayerPrefs.Save();
    }

    public void LoadPlotData(int slotIndex)
    {
        for (int i = 0; i < allPlots.Length; i++)
        {
            int isLockedValue = PlayerPrefs.GetInt($"Plot_{slotIndex}_{i}_IsLocked", 1);
            allPlots[i].isLocked = isLockedValue == 1;
            allPlots[i].SetPlotColor(allPlots[i].isLocked);
        }
        
    }

    #endregion

    #region Unlock and Reset Plot Data

    public void UnlockFirstPlot()
    {
        if (allPlots.Length > 0)
        {
            allPlots[0].isLocked = false;
            allPlots[0].DeactivateBoundary();
        }
    }

    public void ResetGameData(int slotIndex)
    {
        for (int i = 0; i < allPlots.Length; i++)
        {
            PlayerPrefs.DeleteKey($"Plot_{slotIndex}_{i}_IsLocked");
            allPlots[i].isLocked = true;
            allPlots[i].ActivateBoundary();
            allPlots[i].SetPlotColor(allPlots[i].isLocked);
        }
        UnlockFirstPlot();
    }

    #endregion
}