using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CallButtonController : MonoBehaviour
{
    private static readonly int buttonTriggerAnimID = Animator.StringToHash("Click");

    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private TextMeshPro floorIndicator;

    public UnityEvent OnButtonInteract;

    private bool playerInRange = false;

    private void Awake()
    {
        playerInputManager.PlayerInput.PlayerInteract.Enable();
    }

    private void OnEnable()
    {
        playerInputManager.PlayerInput.PlayerInteract.Interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        playerInputManager.PlayerInput.PlayerInteract.Interact.performed -= OnInteractPerformed;
    }

    public void SetPlayerInRange(bool value)
    {
        playerInRange = value;
    }

    public void SetFloorNumber(int floor)
    {
        floorIndicator.text = floor.ToString();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (playerInRange && context.performed)
        {
            buttonAnimator.SetTrigger(buttonTriggerAnimID);
            OnButtonInteract?.Invoke();
        }
    }
}
