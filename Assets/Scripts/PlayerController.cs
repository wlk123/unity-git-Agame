using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5;
    private CharacterController _characterController;
    private PlayerInputManager _playerInputManager;

    private void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 targetDirection=new Vector3(_playerInputManager.move.x,0,_playerInputManager.move.y);
        _characterController.Move(targetDirection*Speed*Time.deltaTime);
    }
}
