using UnityEngine;
using Cinemachine;
 
[SaveDuringPlay]
[AddComponentMenu("")]
public class CinemachineLockToXYOffset : CinemachineExtension
{
    private float _zOffset;
    private float _yOffset;
 
    private InputManager _input;
    
    
    void Start()
    {
        _input = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;

    }
    protected override void Awake()
    {
        base.Awake();
 
        if (!VirtualCamera || VirtualCamera is not CinemachineVirtualCamera virtualCamera)
        {
            return;
        }
 
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
 
        if (!transposer)
        {
            return;
        }
 
        var offset = transposer.m_FollowOffset;
 
        _zOffset = offset.z;
        _yOffset = offset.y;
    }
 
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var position = state.RawPosition;
 
            position.z = _zOffset;
            position.y = _yOffset;
 
            state.RawPosition = position;
        }
    }
}