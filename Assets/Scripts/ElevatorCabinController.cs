using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCabinController : MonoBehaviour
{
    [SerializeField] private DoorController doorController;
    [SerializeField] private ElevatorCabinPanelController elevatorCabinPanelController;
    [SerializeField] private AudioSource engineAudioSource;

    public DoorController DoorController => doorController;
    public ElevatorCabinPanelController ElevatorCabinPanelController => elevatorCabinPanelController;

    public void ToggleEngineSound(bool value)
    {
        if (value)
        {
            engineAudioSource.Play();
        }
        else
        {
            engineAudioSource.Stop();
        }
    }
}
