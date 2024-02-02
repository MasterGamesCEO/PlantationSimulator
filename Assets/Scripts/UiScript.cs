using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScript : MonoBehaviour
{
    [SerializeField] public GameObject Plopup;
    Animator m_Animator;
    public void Start()
    {
        m_Animator = Plopup.GetComponent<Animator>();
    }

    public void animatePlopup()
    {
        m_Animator.SetTrigger("Plopup");
    }
    
}
