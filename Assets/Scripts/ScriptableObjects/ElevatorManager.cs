using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ElevatorCallEvent : UnityEvent<DoorFrameController> { }

[CreateAssetMenu(fileName = "Elevator Manager")]
public class ElevatorManager : ScriptableObject
{
   public ElevatorCallEvent ElevatorCallEvent = new ElevatorCallEvent();
   public float ElevatorSpeed;
   public bool EnRoute;
   public void CallElevator(DoorFrameController doorFrameController)
   {
      if (!EnRoute)
      {
         ElevatorCallEvent?.Invoke(doorFrameController);
      }
   }
}
