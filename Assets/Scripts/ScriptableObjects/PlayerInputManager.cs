using System.Collections;
using System.Collections.Generic;
using Player.Input;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Input Manager")]
public class PlayerInputManager : ScriptableObject
{
    public PlayerInput PlayerInput;

    private void OnEnable()
    {
        PlayerInput = new PlayerInput();
    }
}
