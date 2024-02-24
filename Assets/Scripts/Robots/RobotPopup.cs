using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPopup : MonoBehaviour
{
    private Animator _mAnimator;

    private static readonly int In = Animator.StringToHash("In");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivatePopup(float plotPriceValue)
    {
        _mAnimator.SetBool(In, true);
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
