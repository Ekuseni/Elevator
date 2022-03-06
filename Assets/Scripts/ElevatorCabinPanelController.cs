using System.Collections;
using System.Collections.Generic;
using Player.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ElevatorCabinPanelController : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private Canvas buttonCanvas;
    [SerializeField] private RectTransform buttonContainer;
    [SerializeField] private TMPro.TextMeshProUGUI floorIndicator;
    [SerializeField] private List<Button> floorButtons;
    [SerializeField] private Button openDoorsButton;
    [SerializeField] private Button closeDoorsButton;
    [SerializeField] private AudioSource buttonClicksSource;

    private bool playerInRange = false;
    private bool interactingWithPanel = false;

    private void Awake()
    {
        playerInputManager.PlayerInput.PlayerInteract.Enable();

        foreach (var floorButton in floorButtons)
        {
            floorButton.onClick.AddListener(() =>
            {
                buttonClicksSource.Play();
                ToggleState();
            });
        }

        closeDoorsButton.onClick.AddListener(() =>
        {
            buttonClicksSource.Play();
            ToggleState();
        });

        openDoorsButton.onClick.AddListener(() =>
        {
            buttonClicksSource.Play();
            ToggleState();
        });
    }

    private void OnEnable()
    {
        playerInputManager.PlayerInput.PlayerInteract.Interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        playerInputManager.PlayerInput.PlayerInteract.Interact.performed -= OnInteractPerformed;
    }

    private void Start()
    {
        buttonCanvas.worldCamera = playerInputManager.PlayerCamera;
    }

    public void SetUp(ElevatorShaftController elevatorShaftController)
    {
        for (int i = 0; i < elevatorShaftController.FloorCount; i++)
        {
            floorButtons[i].gameObject.SetActive(true);
            var floor = i;
            floorButtons[i].onClick.AddListener(() => elevatorShaftController.GoToFloor.Invoke(floor));
        }

        elevatorShaftController.MovedToFloor.AddListener((floor) => floorIndicator.text = floor.ToString());

        closeDoorsButton.onClick.AddListener(() =>
        {
            elevatorShaftController.ToggleDoors.Invoke(false);
        });

        openDoorsButton.onClick.AddListener(() =>
        {
            elevatorShaftController.ToggleDoors.Invoke(true);
        });
    }

    public void SetPlayerInRange(bool value)
    {
        playerInRange = value;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (playerInRange && context.performed)
        {
            ToggleState();
        }
    }

    private void ToggleState()
    {
        if (interactingWithPanel)
        {
            playerInputManager.PlayerInput.PlayerLook.Enable();
            playerInputManager.PlayerInput.PlayerMove.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            playerInputManager.PlayerInput.PlayerLook.Disable();
            playerInputManager.PlayerInput.PlayerMove.Disable();
            Cursor.lockState = CursorLockMode.None;
        }

        interactingWithPanel = !interactingWithPanel;
    }
}
