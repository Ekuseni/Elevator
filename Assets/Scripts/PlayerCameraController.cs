using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Player.Input.PlayerInput;

public class PlayerCameraController : MonoBehaviour, PlayerInput.IPlayerLookActions
{
    [SerializeField] private float lookSensitivity = 100f;
    [SerializeField] private Transform playerTransform;

    private float xRotation = 0f;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.PlayerLook.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        var cameraMovement = playerInput.PlayerLook.Look.ReadValue<Vector2>() * lookSensitivity * Time.deltaTime;

        xRotation -= cameraMovement.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * cameraMovement.x);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Debug.Log(context.action.ReadValue<Vector2>());
    }
}
