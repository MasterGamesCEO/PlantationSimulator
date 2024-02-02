using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;



public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private InputManager input;
    


    [SerializeField]
    private float Speed = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.instance;
        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement(Time.deltaTime);
    }

    private void HandleMovement(float delta)
    {
        //multiplying by transform.right and transform.forward will make all movement relative to camera direction. 
        Vector3 movement = (input.Move.x * transform.right) + (input.Move.y * transform.forward);
        
        controller.Move(movement * (Speed * delta));
    }

    
    private void OnTriggerEnter(Collider other)
    {
        UiScript _uiScript = gameObject.GetComponent<UiScript>();
        if (other.gameObject.tag.Equals("Plot"))
        {
            PlotStats _stats = other.gameObject.GetComponent<PlotStats>();
            if (_stats.isLocked == true)
            {
                Debug.Log("LOCKED PLOT");
                if (_uiScript.Plopup.GetComponent<Animator>())
                {
                    
                }
                _uiScript.animatePlopup();
            }
            else
            {
                Debug.Log("UNLOCKED PLOT");
                _stats.deactivateBoundry();
                
            }
            
        }
        else
        {
            
        }
    }
}
