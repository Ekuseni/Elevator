using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Player.Input.PlayerInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;

    [SerializeField] private float playerSpeed;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float jumpHeight;

    private readonly float gravity = Physics.gravity.y;
    private float verticalVelocity;

    private void Awake()
    {
        playerInputManager.PlayerInput.PlayerMove.Enable();
        playerInputManager.PlayerTransform = transform;

        playerInputManager.MovePlayer = characterController.Move;
    }

    private void Update()
    {
        var playerMovement = playerInputManager.PlayerInput.PlayerMove.Move.ReadValue<Vector2>() * playerSpeed;

        if (characterController.isGrounded)
        {
            verticalVelocity = gravity * 0.1f;

            if (playerInputManager.PlayerInput.PlayerMove.Jump.triggered)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -gravity * 2f);
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 moveVector = transform.right * playerMovement.x + transform.forward * playerMovement.y + transform.up * verticalVelocity;
        characterController.Move(moveVector * Time.deltaTime);
    }
}
