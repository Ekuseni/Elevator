using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCabinController : MonoBehaviour
{
    [SerializeField] private DoorController doorController;
    [SerializeField] private ElevatorCabinPanelController elevatorCabinPanelController;

    public DoorController DoorController => doorController;
    public ElevatorCabinPanelController ElevatorCabinPanelController => elevatorCabinPanelController;
}
