using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Diagnostics.CodeAnalysis;
using SaveLoad;

public class UnassignPopup : MonoBehaviour
{
    private Animator _mAnimator;
    [SerializeField] public Platform selectedPlatform;
    private static readonly int In = Animator.StringToHash("In");
    private InputManager _input;

    // Start is called before the first frame update
    void Start()
    {
        _input = InputManager.Instance;
        _mAnimator = GetComponent<Animator>();
        _input.quickSell.performed += OnUnAssignRobotPreformed;
        _input.unAssignRobot.performed += OnUnAssignRobotPreformed;
    }

    private void OnUnAssignRobotPreformed(InputAction.CallbackContext obj)
    {
        if (_input.quickSell.WasPressedThisFrame()) 
        {
            if (PopupActive())
            {
                Debug.Log(selectedPlatform);
                RobotManager manager = FindObjectOfType<RobotManager>();
                PlatformDataHandler handler = FindObjectOfType<PlatformDataHandler>();
                if (manager != null && handler != null)
                {
                    //TODO: Remove from working robots the stats
                    RobotAttributes attributes = new RobotAttributes()
                    {
                        cps = selectedPlatform.stats.cps,
                        quickSellPrice = selectedPlatform.stats.quickSellPrice,
                        robotType = selectedPlatform.stats.robotType,
                        size = selectedPlatform.stats.size,
                        price = selectedPlatform.stats.price,
                        ID = selectedPlatform.stats.ID
                    };
                    if (attributes != null)
                    {
                        for (int i = 0; i < manager.workingRobots.Count; i++)
                        {
                            if ((manager.workingRobots[i].cps == attributes.cps) &&
                                (manager.workingRobots[i].quickSellPrice == attributes.quickSellPrice) &&
                                (manager.workingRobots[i].robotType == attributes.robotType) &&
                                (manager.workingRobots[i].size == attributes.size) && manager.workingRobots[i].ID == attributes.ID)
                            {
                                manager.workingRobots.Remove(manager.workingRobots[i]);
                                selectedPlatform.stats.isAssigned = false;
                                selectedPlatform.RemoveRobotFromScene();
                                
                                
                                PlotPricePopupScript plotPricePopupScript = FindObjectOfType<PlotPricePopupScript>();
                                plotPricePopupScript.UpdateMoney(-(selectedPlatform.stats.quickSellPrice));
                                PlayerController playerController = FindObjectOfType<PlayerController>();
                                playerController.SetPlayerMoney(CurrentData.Instance.uiData.saveMoney + selectedPlatform.stats.quickSellPrice);
                                DeactivatePopup();
                            }
                            else
                            {
                                Debug.Log("No match in run");
                            }
                        }
                    }
                }
            }
        } else if (_input.unAssignRobot.WasPressedThisFrame())
        {
            //TODO: move the stats back to unassigned
            if (PopupActive())
            {
                Debug.Log(selectedPlatform);
                RobotManager manager = FindObjectOfType<RobotManager>();
                PlatformDataHandler handler = FindObjectOfType<PlatformDataHandler>();
                if (manager != null && handler != null)
                {
                    RobotAttributes attributes = new RobotAttributes()
                    {
                        cps = selectedPlatform.stats.cps,
                        quickSellPrice = selectedPlatform.stats.quickSellPrice,
                        robotType = selectedPlatform.stats.robotType,
                        size = selectedPlatform.stats.size,
                        price = selectedPlatform.stats.price, ID = selectedPlatform.stats.ID
                    };
                    if (attributes != null)
                    {
                        for (int i = 0; i < manager.workingRobots.Count; i++)
                        {
                            if ((manager.workingRobots[i].cps == attributes.cps) &&
                                (manager.workingRobots[i].quickSellPrice == attributes.quickSellPrice) &&
                                (manager.workingRobots[i].robotType == attributes.robotType) &&
                                (manager.workingRobots[i].size == attributes.size) && manager.workingRobots[i].ID == attributes.ID)
                            {
                                manager.unassignedRobots.Add(manager.workingRobots[i]);
                                manager.workingRobots.Remove(manager.workingRobots[i]);
                                selectedPlatform.stats.isAssigned = false;
                                selectedPlatform.RemoveRobotFromScene();
                                
                                DeactivatePopup();
                            }
                            else
                            {
                                Debug.Log("No match in run");
                            }
                        }
                    }
                }
            }
        }

        
    }

    public void ActivatePopup(Platform platform)
    {
        RobotManager manager = FindObjectOfType<RobotManager>();
        Transform option = _mAnimator.transform.Find($"Slot{0}");
        TextMeshProUGUI optionText = option.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();
        if (optionText != null)
            optionText.text = "$" + platform.stats.quickSellPrice;

        _mAnimator.SetBool(In, true);
        selectedPlatform = platform;
    }

    public void DeactivatePopup()
    {
        _mAnimator.SetBool(In, false);
    }

    public bool PopupActive()
    {
        return _mAnimator.GetBool(In);
    }
}
