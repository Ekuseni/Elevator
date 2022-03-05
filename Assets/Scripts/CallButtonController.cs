using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Player.Input.PlayerInput;

public class CallButtonController : MonoBehaviour, PlayerInput.IPlayerInteractActions
{
    private static int buttonTriggerAnimID = Animator.StringToHash("Click");
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private Animator buttonAnimator;

    private void Awake()
    {
        playerInputManager.PlayerInput.PlayerInteract.SetCallbacks(this);
    }

    public void SetPlayerInRange(bool value)
    {
        if (value)
        {
            playerInputManager.PlayerInput.PlayerInteract.Enable();
            
        }
        else
        {
            playerInputManager.PlayerInput.PlayerInteract.Disable();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buttonAnimator.SetTrigger(buttonTriggerAnimID);
        }
    }
}
