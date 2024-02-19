using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotDataHandler : MonoBehaviour
{
    [SerializeField] private PlayerController _controller;
    [SerializeField] public PlotStats[] allPlots;

    #region Save and Load Plot Data

    public void SavePlotData()
    {
        for (int i = 0; i < allPlots.Length; i++)
        {
            PlayerPrefs.SetInt($"Plot_{i}_IsLocked", allPlots[i].isLocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadPlotData()
    {
        for (int i = 0; i < allPlots.Length; i++)
        {
            int isLockedValue = PlayerPrefs.GetInt($"Plot_{i}_IsLocked", 1);
            allPlots[i].isLocked = isLockedValue == 1;
            allPlots[i].setPlotColor(allPlots[i].isLocked);
        }
    }

    #endregion

    #region Unlock and Reset Plot Data

    public void UnlockFirstPlot()
    {
        if (allPlots.Length > 0)
        {
            allPlots[0].isLocked = false;
            allPlots[0].DeactivateBoundry();
        }
    }

    public void ResetGameData()
    {
        for (int i = 0; i < allPlots.Length; i++)
        {
            allPlots[i].isLocked = true;
            allPlots[i].ActivateBoundry();
        }
        UnlockFirstPlot();
        PlayerPrefs.DeleteAll();
    }

    #endregion
}