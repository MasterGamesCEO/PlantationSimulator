using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InputManager input;
    
    [SerializeField] private Transform playerParent;
    private Coroutine moveRoutine;

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.instance;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    { 
        // turn off the hipfire reticle?
        // interpolate between the original position of the gun and the aim position
        if (playerParent.transform.position.x <= transform.position.x -15 || playerParent.transform.position.x >= transform.position.x + 15 )
        {
            if(moveRoutine != null) StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(MoveCamera());
            
        }
        
        
        
    }
    
    private IEnumerator MoveCamera()
    {
        Vector3 startingX = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 endingXR = new Vector3(playerParent.transform.position.x -15, transform.position.y, transform.position.z);
        Vector3 endingXL = new Vector3(playerParent.transform.position.x +15, transform.position.y, transform.position.z);
        while (true)
        {
            if (playerParent.transform.position.x <= transform.position.x -15 )
            {
                transform.position = Vector3.Lerp(startingX, endingXL ,Time.deltaTime * 8);

            } else if (playerParent.transform.position.x >= transform.position.x + 15)
            {
                transform.position = Vector3.Lerp(startingX, endingXR ,Time.deltaTime * 8);
            }
            yield return null;
        }
    }
}
