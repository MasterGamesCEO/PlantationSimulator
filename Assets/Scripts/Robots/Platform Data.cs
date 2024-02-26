using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformData : MonoBehaviour
{
    [SerializeField] public bool isAssigned = false;
    [SerializeField] public BoxCollider platformPos;
    [SerializeField] public Component noRobotPrefab;
    [SerializeField] public Component assignedRobotPrefab;
    [SerializeField] public PlatformDataHandler platformData;
    
    private Component _currentRobotPrefab;
    
    public void SetRobotPrefab(bool assigned)
    {
        if (!assigned)
        {
            if (_currentRobotPrefab == null)
            {
                _currentRobotPrefab = Instantiate(noRobotPrefab, platformPos.transform);
                platformData.AddToPlatform(_currentRobotPrefab);
            }
        }
    }

    public void SetRobotPrefab(bool assigned, Component newRobotPrefab)
    {
        if (assigned)
        {
            if (_currentRobotPrefab != null)
            {
                Destroy(_currentRobotPrefab.gameObject);
            }
            _currentRobotPrefab = Instantiate(newRobotPrefab, platformPos.transform);
        }
        else
        {
            Debug.LogError("Trying to assign a robot to an unassigned platform.");
        }
    }
}
