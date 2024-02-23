using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    [SerializeField] private Transform newPosition;

    void Start()
    {
        UpdatePosition();
    }

    void Update()
    {
        if (transform.position != newPosition.position)
        {
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        transform.position = newPosition.position;
    }
}