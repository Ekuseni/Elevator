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

   private static int layer = 7;

   private void Awake()
   {
      gameObject.layer = layer;
   }

   private void OnTriggerEnter(Collider other)
   {
      OnPlayerEnter?.Invoke();
   }

   private void OnTriggerExit(Collider other)
   {
      OnPlayerExit?.Invoke();
   }
}
