using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions playerInput;
    private InputSystem_Actions.PlayerActions playerActions;

    private PlayerMotor motor;
    private PlayerAttack attack;
    private PlayerSpecial special;
    private PlayerGrabnThrow grabnThrow;
    private PlayerUlt Ult;
    private BlockScript blockScript;

    void Awake()
    {

        playerInput = new InputSystem_Actions();
        playerActions = playerInput.Player;

        motor = GetComponent<PlayerMotor>();
        blockScript = GetComponent<BlockScript>();
        attack = GetComponent<PlayerAttack>();
        special = GetComponent<PlayerSpecial>();
        grabnThrow = GetComponent<PlayerGrabnThrow>();
        Ult = GetComponent<PlayerUlt>();

        playerActions.Jump.performed += ctx => motor.Jump();
        playerActions.Attack.performed += ctx => attack.PerformAttack();
        playerActions.Special.performed += ctx => special.PerformSpecial();
        playerActions.GrabnThrow.performed += ctx => grabnThrow.PerformGrabnThrow();
        playerActions.Ulti.performed += ctx => Ult.PerformUlt();
    }

    // Update is called once per frame
    void Update()
    {
        motor.ProcessMove(playerActions.Move.ReadValue<Vector2>());
        blockScript.PerformDeffend(playerActions.Block.ReadValue<float>() > 0);
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
