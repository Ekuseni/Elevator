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

public class ElevatorShaftController : MonoBehaviour
{
    [SerializeField] private ElevatorCabinController elevatorCabin;
    [SerializeField] private List<ElevatorShaftDoorController> elevatorShaftDoors;
    [SerializeField] private float elevatorSpeed;
    [SerializeField] private float floorHeight;

    public ElevatorCallEvent ElevatorCallEvent = new ElevatorCallEvent();
    public MovedToFloorEvent MovedToFloor = new MovedToFloorEvent();
    private bool enRoute;


    private void Awake()
    {
        foreach (var elevatorShaftDoor in elevatorShaftDoors)
        {
            elevatorShaftDoor.SetUp(this);
        }

        ElevatorCallEvent.AddListener(CallElevator);

        MovedToFloor?.Invoke(Mathf.FloorToInt((elevatorCabin.transform.localPosition.y + floorHeight / 2f) / floorHeight));
    }

    public void CallElevator(ElevatorShaftDoorController elevatorShaftDoorController)
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

        yield return new WaitUntil(() => elevatorCabin.DoorController.Closed);
        yield return new WaitUntil(() => elevatorShaftDoorController.DoorController.Closed);

        while (Vector3.Distance(elevatorCabin.transform.position, elevatorShaftDoorController.ElevatorTarget.position) > 0.1f)
        {
            elevatorCabin.transform.position =
                Vector3.MoveTowards(elevatorCabin.transform.position, elevatorShaftDoorController.ElevatorTarget.position, elevatorSpeed * Time.deltaTime);
            
            MovedToFloor?.Invoke(Mathf.FloorToInt((elevatorCabin.transform.localPosition.y + floorHeight / 2f) / floorHeight));

            yield return null;
        }

        elevatorCabin.transform.position = elevatorShaftDoorController.ElevatorTarget.position;

        elevatorShaftDoorController.DoorController.SetOpen(true);
        elevatorCabin.DoorController.SetOpen(true);

        enRoute = false;

        yield return null;
    }
}
