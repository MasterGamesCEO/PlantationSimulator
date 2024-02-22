using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] public UIManager uiManager;
    private Animator _mAnimator;
    private static readonly int DialogIsOpen = Animator.StringToHash("DialogIsOpen");

    private void Start()
    {
        _mAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (uiManager.isDialogOpen)
        {
            _mAnimator.SetBool(DialogIsOpen, true);
        }
        else
        {
            _mAnimator.SetBool(DialogIsOpen, false);
        }
    }
}
