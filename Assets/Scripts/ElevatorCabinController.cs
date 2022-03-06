using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCabinController : MonoBehaviour
{
    [SerializeField] private DoorController doorController;

    public DoorController DoorController => doorController;
}
