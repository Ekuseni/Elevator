using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[Serializable]
public class PlayerInWayEvent : UnityEvent<bool> { }

[Serializable]
public class ToggleDoorsEvent : UnityEvent<bool> { }

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
    
    public MovedToFloorEvent MovedToFloor = new MovedToFloorEvent();
    public GoToFloorEvent GoToFloor = new GoToFloorEvent();
    public ToggleDoorsEvent ToggleDoors = new ToggleDoorsEvent();

    public int FloorCount => elevatorShaftDoors.Count;

    private bool enRoute;

    private int currentFloor => Mathf.FloorToInt((elevatorCabin.transform.localPosition.y + floorHeight / 2f) / floorHeight);

    private void Awake()
    {
        for (int i = 0; i < FloorCount; i++)
        {
            elevatorShaftDoors[i].SetUp(this, i);
        }

        elevatorCabin.ElevatorCabinPanelController.SetUp(this);

        GoToFloor.AddListener(OnGoToFloor);
        ToggleDoors.AddListener(OnToggleDoors);
        MovedToFloor?.Invoke(Mathf.FloorToInt((elevatorCabin.transform.localPosition.y + floorHeight / 2f) / floorHeight));
    }

    private void OnGoToFloor(int floor)
    {
        if (!enRoute)
        {
            StartCoroutine(GoToTargetAndOpenDoors(floor));
        }
    }

    private void OnToggleDoors(bool value)
    {
        elevatorShaftDoors[currentFloor].DoorController.SetOpen(value);
        elevatorCabin.DoorController.SetOpen(value);
    }

    public void OnPlayerInWay(bool value)
    {
        elevatorCabin.DoorController.SetPlayerInWay(value);
    }

    private IEnumerator GoToTargetAndOpenDoors(int floor)
    {
        var elevatorShaftDoorController = elevatorShaftDoors[floor];

        if (currentFloor != floor)
        {
            enRoute = true;
            
            OnToggleDoors(false);

            yield return new WaitUntil(() => elevatorCabin.DoorController.Closed);
            yield return new WaitUntil(() => elevatorShaftDoorController.DoorController.Closed);
            
            elevatorCabin.ToggleEngineSound(true);

            int lastFloor = currentFloor;

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
        }

        elevatorCabin.transform.position = elevatorShaftDoorController.ElevatorTarget.position;
        elevatorCabin.ToggleEngineSound(false);
        OnToggleDoors(true);

        enRoute = false;

        yield return null;
    }
}
