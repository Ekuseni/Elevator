using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Player.Input.PlayerInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float jumpHeight;

    private readonly float gravity = Physics.gravity.y;
    private float verticalVelocity;

    private PlayerInput playerInput; 

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.PlayerMove.Enable();
    }

    private void Update()
    {
        var playerMovement = playerInput.PlayerMove.Move.ReadValue<Vector2>() * playerSpeed;

        if (characterController.isGrounded)
        {
            verticalVelocity = gravity * 0.1f;

            if (playerInput.PlayerMove.Jump.triggered)
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
