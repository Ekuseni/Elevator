using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using PlayerInput = Player.Input.PlayerInput;

[RequireComponent(typeof(Collider))]
public class PlayerDetector : MonoBehaviour
{
   public UnityEvent OnPlayerEnter;
   public UnityEvent OnPlayerExit;

   private void OnTriggerEnter(Collider other)
   {
      Debug.Log(other);

      OnPlayerEnter?.Invoke();
   }

   private void OnTriggerExit(Collider other)
   {
      Debug.Log(other);

      OnPlayerExit?.Invoke();
   }
}
