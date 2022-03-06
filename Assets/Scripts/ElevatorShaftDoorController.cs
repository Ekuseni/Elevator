using UnityEngine;

public class ElevatorShaftDoorController : MonoBehaviour
{
    [SerializeField] private CallButtonController callButtonController;
    [SerializeField] private DoorController doorController;
    [SerializeField] private Transform elevatorTarget;

    public DoorController DoorController => doorController;
    public Transform ElevatorTarget => elevatorTarget;

    private int floorNumber;

    public void SetUp(ElevatorShaftController elevatorShaftController, int floorNumber)
    {
        callButtonController.OnButtonInteract.AddListener(() => elevatorShaftController.GoToFloor.Invoke(floorNumber));
        elevatorShaftController.MovedToFloor.AddListener(callButtonController.SetFloorNumber);
        doorController.PlayerInWay.AddListener(elevatorShaftController.OnPlayerInWay);
    }
}
