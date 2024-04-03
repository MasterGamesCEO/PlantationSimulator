using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlotDataHandler : MonoBehaviour
{
    [SerializeField] public List<Plot> allPlots;
    
    

    
    #region Save and Load Plot Data

    public void SavePlotData()
    {
        List<PlotStats> savePlotStats = new List<PlotStats>();
        for (int i = 0; i < allPlots.Count; i++)
        {
            savePlotStats.Add(allPlots[i].GetPlotStats());
        }

        CurrentData.Instance.gameplayData.gameplayPlotStats = savePlotStats;
    }

    public void LoadPlotData()
    {
        
        for (int i = 0; i < allPlots.Count; i++)
        {
            allPlots[i].SetPlotStats(CurrentData.Instance.gameplayData.gameplayPlotStats[i]);
            allPlots[i].SetPlotColor(allPlots[i].stats.isLocked);
        }
        
    }

    #endregion
    
    
    public List<PlotStats> ResetPlotData()
    {
        List<PlotStats> savePlotStats = new List<PlotStats>();
        allPlots[0].stats.isLocked = false;
        allPlots[0].DeactivateBoundary();
        for (int i = 1; i < allPlots.Count; i++)
        {
            allPlots[i].stats.isLocked= true;
            allPlots[i].ActivateBoundary();
            allPlots[i].SetPlotColor(allPlots[i].stats.isLocked);
            savePlotStats.Add(allPlots[i].GetPlotStats());
        }
        
        return savePlotStats;
    }
    
}