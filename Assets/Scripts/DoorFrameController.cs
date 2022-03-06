using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFrameController : MonoBehaviour
{
    [SerializeField] private ElevatorManager elevatorManager;
    [SerializeField] private CallButtonController callButtonController;
    [SerializeField] private DoorController doorController;
    [SerializeField] private Transform elevatorTarget;

    public DoorController DoorController => doorController;
    public Transform ElevatorTarget => elevatorTarget;

    private void Awake()
    {
        callButtonController.OnButtonInteract.AddListener(PerformOnCallActions);
    }

    private void PerformOnCallActions()
    {
        elevatorManager.ElevatorCallEvent.Invoke(this);
    }
}
