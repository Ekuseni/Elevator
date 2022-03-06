using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorShaftDoorController : MonoBehaviour
{
    [SerializeField] private CallButtonController callButtonController;
    [SerializeField] private DoorController doorController;
    [SerializeField] private Transform elevatorTarget;

    public DoorController DoorController => doorController;
    public Transform ElevatorTarget => elevatorTarget;

    public void SetUp(ElevatorShaftController elevatorShaftController)
    {
        callButtonController.OnButtonInteract.AddListener(() => elevatorShaftController.ElevatorCallEvent.Invoke(this));
        elevatorShaftController.MovedToFloor.AddListener(callButtonController.SetFloorNumber);
        doorController.PlayerInWay.AddListener(elevatorShaftController.OnPlayerInWay);
    }
}
