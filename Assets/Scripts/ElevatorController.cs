using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] private ElevatorManager elevatorManager;
    [SerializeField] private DoorController doorController;

    private DoorController otherDoorController;

    private IEnumerable<DoorController> doorControllers
    {
        get
        {
            yield return doorController;
            yield return otherDoorController;
        }
    }

    private void Awake()
    {
        elevatorManager.ElevatorCallEvent.AddListener(OnElevatorCall);
    }

    private void OnElevatorCall(DoorFrameController doorFrameController)
    {
        StopAllCoroutines();
        otherDoorController = doorFrameController.DoorController;
        StartCoroutine(GoToTargetAndOpenDoors(doorFrameController.ElevatorTarget));
    }

    private IEnumerator GoToTargetAndOpenDoors(Transform targetTransform)
    {
        elevatorManager.EnRoute = true;

        while (Vector3.Distance(transform.position, targetTransform.position) > 0.1f)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, targetTransform.position, elevatorManager.ElevatorSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = targetTransform.position;

        foreach (var door in doorControllers)
        {
            door.SetOpen(true);
        }

        elevatorManager.EnRoute = false;
    }
}
