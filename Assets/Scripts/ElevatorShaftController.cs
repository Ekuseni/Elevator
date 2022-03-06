using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ElevatorCallEvent : UnityEvent<ElevatorShaftDoorController> { }

[Serializable]
public class PlayerInWayEvent : UnityEvent<bool> { }

[Serializable]
public class MovedToFloorEvent : UnityEvent<int> { }

[Serializable]
public class GoToFloorEvent : UnityEvent<int> { }

public class ElevatorShaftController : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private ElevatorCabinController elevatorCabin;
    [SerializeField] private List<ElevatorShaftDoorController> elevatorShaftDoors;
    [SerializeField] private float elevatorSpeed;
    [SerializeField] private float floorHeight;

    public ElevatorCallEvent ElevatorCallEvent = new ElevatorCallEvent();
    public MovedToFloorEvent MovedToFloor = new MovedToFloorEvent();
    public GoToFloorEvent GoToFloor = new GoToFloorEvent();

    public int FloorCount => elevatorShaftDoors.Count;

    private bool enRoute;

    private int currentFloor => Mathf.FloorToInt((elevatorCabin.transform.localPosition.y + floorHeight / 2f) / floorHeight);

    private void Awake()
    {
        foreach (var elevatorShaftDoor in elevatorShaftDoors)
        {
            elevatorShaftDoor.SetUp(this);
        }

        elevatorCabin.ElevatorCabinPanelController.SetUp(this);

        ElevatorCallEvent.AddListener(OnCallElevator);
        GoToFloor.AddListener(OnGoToFloor);
        MovedToFloor?.Invoke(Mathf.FloorToInt((elevatorCabin.transform.localPosition.y + floorHeight / 2f) / floorHeight));
    }

    private void OnGoToFloor(int floor)
    {
        OnCallElevator(elevatorShaftDoors[floor]);
    }

    private void OnCallElevator(ElevatorShaftDoorController elevatorShaftDoorController)
    {
        if (!enRoute)
        {
            StartCoroutine(GoToTargetAndOpenDoors(elevatorShaftDoorController));
        }
    }

    public void OnPlayerInWay(bool value)
    {
        elevatorCabin.DoorController.SetPlayerInWay(value);
    }

    private IEnumerator GoToTargetAndOpenDoors(ElevatorShaftDoorController elevatorShaftDoorController)
    {
        enRoute = true;
        int lastFloor = currentFloor;

        yield return new WaitUntil(() => elevatorCabin.DoorController.Closed);
        yield return new WaitUntil(() => elevatorShaftDoorController.DoorController.Closed);

        Vector3 lastPos = elevatorCabin.transform.position;

        while (Vector3.Distance(elevatorCabin.transform.position, elevatorShaftDoorController.ElevatorTarget.position) > 0.1f)
        {
            elevatorCabin.transform.position =
                Vector3.MoveTowards(elevatorCabin.transform.position, elevatorShaftDoorController.ElevatorTarget.position, elevatorSpeed * Time.deltaTime);

            if (playerInputManager.InsideElevator)
            {
                var additionalMoveVector = elevatorCabin.transform.position - lastPos;
                Debug.Log(additionalMoveVector);
                playerInputManager.MovePlayer(additionalMoveVector);
                lastPos = elevatorCabin.transform.position;
            }

            if (currentFloor != lastFloor)
            {
                lastFloor = currentFloor;
                MovedToFloor?.Invoke(currentFloor);
            }

            yield return null;
        }

        elevatorCabin.transform.position = elevatorShaftDoorController.ElevatorTarget.position;

        elevatorShaftDoorController.DoorController.SetOpen(true);
        elevatorCabin.DoorController.SetOpen(true);

        enRoute = false;

        yield return null;
    }
}
