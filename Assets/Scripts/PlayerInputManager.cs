using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
   public Vector2 move;
   private void OnMove(InputValue value)
   {
      move = value.Get<Vector2>();
   }
   
}
