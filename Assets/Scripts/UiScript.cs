using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiScript : MonoBehaviour
{
    [SerializeField] public GameObject Plopup;
    [SerializeField] TextMeshProUGUI plotPrice;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] int moneyPrice;
    Animator m_Animator;
    
    public void Start()
    {
        m_Animator = Plopup.GetComponent<Animator>();
    }
    

    public void activatePopup()
    {
        plotPrice.text = "2000";
        m_Animator.SetBool("popup", true);
    }

    public void deactivatePopup()
    {
        m_Animator.SetBool("popup", false);
    }

    public bool popupActive()
    {
        return m_Animator.GetBool("popup");
    }

    public void updateMoney(String text)
    {
        
        moneyText.text = text;
    }
    

}
