using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions playerInput;
    private InputSystem_Actions.PlayerActions playerActions;

    private PlayerMotor motor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new InputSystem_Actions();
        playerActions = playerInput.Player;
        motor = GetComponent<PlayerMotor>();
   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ProcessMove(playerActions.Move.ReadValue<Vector2>().normalized);
    }
    private void OnEnable()
    {
        playerActions.Enable();
    }
    private void OnDisable() 
    {
        playerActions.Disable();
    }
}
