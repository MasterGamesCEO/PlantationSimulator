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

    public Component RobotPrefab
    {
        get { return _robotPrefab; }
        set
        {
            if (value != _robotPrefab)
            {
                if (_currentRobotPrefab != null)
                {
                    Destroy(_currentRobotPrefab.gameObject);
                }
                _robotPrefab = value;
            }
        }
    }

    private Component _robotPrefab;
    private Component _currentRobotPrefab;
    
    public void SetRobotPrefab(Component newRobotPrefab)
    {
        if (!isAssigned)
        {
            if (_currentRobotPrefab != null)
            {
                Destroy(_currentRobotPrefab.gameObject);
            }
            _currentRobotPrefab = Instantiate(newRobotPrefab, platformPos.transform);
        }
        else
        {
            Debug.LogError("Trying to assign a robot to an assigned platform.");
        }
    }
}
