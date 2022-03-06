using System.Collections;
using System.Collections.Generic;
using Player.Input;
using UnityEngine;
using UnityEngine.Animations;

public delegate CollisionFlags MovePlayerDelegate(Vector3 moveVector);

[CreateAssetMenu(fileName = "Player Input Manager")]
public class PlayerInputManager : ScriptableObject
{
    public PlayerInput PlayerInput { get; private set; }
    public Camera PlayerCamera { get; set; }
    public Transform PlayerTransform { get; set; }
    public bool InsideElevator { get; set; }

    public MovePlayerDelegate MovePlayer { get; set; }

    private void OnEnable()
    {
        PlayerInput = new PlayerInput();
    }

    public void SetInsideElevator(bool value)
    {
        InsideElevator = value;
    }
}
