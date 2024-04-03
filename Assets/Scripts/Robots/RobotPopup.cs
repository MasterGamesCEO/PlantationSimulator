using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RobotPopup : MonoBehaviour
{
    [SerializeField] public Animator _mAnimator;
    [SerializeField] public Platform selectedPlatform;
    private static readonly int In = Animator.StringToHash("In");
    private InputManager _input;

    // Start is called before the first frame update
    void Start()
    {
        _input = InputManager.Instance;
        _mAnimator = GetComponent<Animator>();
        _input.robot1.performed += OnAssignRobotPreformed;
        _input.robot2.performed += OnAssignRobotPreformed;
        _input.robot3.performed += OnAssignRobotPreformed;
        _input.robot4.performed += OnAssignRobotPreformed;
    }

    private void OnAssignRobotPreformed(InputAction.CallbackContext obj)
    {
        int slotIndex = new int();
        if (_input.robot1.WasPressedThisFrame()) {
            slotIndex = 0;
        } else if (_input.robot2.WasPressedThisFrame()) {
            slotIndex = 1;
        } else if (_input.robot3.WasPressedThisFrame()) {
            slotIndex = 2;
        } else if (_input.robot4.WasPressedThisFrame()) {
            slotIndex = 3;
        }
        if (PopupActive())
        {
           Debug.Log(slotIndex);
           RobotManager manager = FindObjectOfType<RobotManager>();
           PlatformDataHandler handler = FindObjectOfType<PlatformDataHandler>();
           if (manager != null && handler != null)
           {
               if (slotIndex < manager.unassignedRobots.Count)
               {
                   manager.workingRobots.Add(manager.unassignedRobots[slotIndex ]);
                   selectedPlatform.stats = new PlatformStats(true, manager.unassignedRobots[slotIndex ]);
                   selectedPlatform.AddRobotToScene();
                   DeactivatePopup();
                   manager.unassignedRobots.Remove(manager.unassignedRobots[slotIndex]);
                   
               }
               else
               {
                   Debug.Log($"No robot in {slotIndex + 1}'s unassigned robots");
               }
           }
        }
    }

    public void ActivatePopup(Platform platform)
    {
        RobotManager manager = FindObjectOfType<RobotManager>();
        for (int i = 0; i < 4; i++)
        {
            int maxIndex = new int();
            maxIndex = manager.unassignedRobots.Count;
            Transform option = _mAnimator.transform.Find($"Slot{i}");
            TextMeshProUGUI optionText = option.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();
            if (optionText != null)
                optionText.text = i < maxIndex
                    ? $"[{i + 1}] " + manager.unassignedRobots[i].robotType
                    : $"[{i + 1}] NO ROBOT";
            Image image = option.Find("Shadow")?.GetComponent<Image>();
            ColorUtility.TryParseHtmlString("#FF6D6D", out var colorRed);
            ColorUtility.TryParseHtmlString("#4CAAFF", out var colorBlue);
            if (image != null) 
                image.color = i < maxIndex 
                    ? colorBlue 
                    : colorRed;
        } 
        _mAnimator.SetBool(In, true);
        selectedPlatform = platform;
    }

    public void DeactivatePopup()
    {
        _mAnimator.SetBool(In, false);
        selectedPlatform = null;
    }

    public bool PopupActive()
    {
        return _mAnimator.GetBool(In);
    }
}
