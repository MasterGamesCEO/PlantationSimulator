using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlotDataHandler : MonoBehaviour
{
    [SerializeField] public List<PlotStats> allPlots;
    

    
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
                allPlots[i].SetPlotColor(allPlots[i].isLocked);
        }
        
    }

    #endregion
    
    
    public List<PlotStats> ResetPlotData()
    {
        for (int i = 0; i < allPlots.Count; i++)
        {
            allPlots[i].isLocked = true;
            allPlots[i].ActivateBoundary();
            allPlots[i].SetPlotColor(allPlots[i].isLocked);
        }
        allPlots[0].isLocked = false;
        allPlots[0].DeactivateBoundary();
        return allPlots;
    }
    
}