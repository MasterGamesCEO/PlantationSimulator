using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InputManager input;
    
    [SerializeField] private Transform playerParent;
    private float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.instance;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
