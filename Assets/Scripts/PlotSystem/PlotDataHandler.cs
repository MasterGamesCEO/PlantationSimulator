using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using UnityEngine;
using UnityEngine.Serialization;

public class PlotDataHandler : MonoBehaviour
{
    [SerializeField] public List<Plot> allPlots;
    

    
    #region Save and Load Plot Data

    public void SavePlotData()
    {
        CurrentData.Instance.PlotStats = allPlots;
    }

    public void LoadPlotData()
    {
        
        allPlots = CurrentData.Instance.PlotStats;
        for (int i = 0; i < allPlots.Count; i++)
        {
                allPlots[i].SetPlotColor(allPlots[i].stats.isLocked);
        }
        
    }

    #endregion
    
    
    public List<Plot> ResetPlotData()
    {
        for (int i = 0; i < allPlots.Count; i++)
        {
            allPlots[i].stats.isLocked= true;
            allPlots[i].ActivateBoundary();
            allPlots[i].SetPlotColor(allPlots[i].stats.isLocked);
        }
        allPlots[0].stats.isLocked = false;
        allPlots[0].DeactivateBoundary();
        return allPlots;
    }
    
}