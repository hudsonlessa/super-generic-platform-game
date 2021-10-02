using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnScreenControls : MonoBehaviour
{
  private PlayerControls playerControls;

  private void Awake()
  {
    playerControls = new PlayerControls();
  }

  private void Update()
  {
    bool hasKeyboardInput = Keyboard.current.anyKey.isPressed;

    if (hasKeyboardInput) Destroy(gameObject);
  }
}
