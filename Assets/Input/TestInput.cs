using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            Debug.Log("Left mouse button pressed");

        if (Keyboard.current.kKey.wasPressedThisFrame)
            Debug.Log("K key pressed");

        if (Keyboard.current.enterKey.wasPressedThisFrame)
            Debug.Log("Enter key pressed");
    }
}

