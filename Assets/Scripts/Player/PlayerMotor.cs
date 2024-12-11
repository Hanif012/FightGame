using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Gerak 
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.z = input.x;
        // mengecek LX
        if (0 < moveDirection.z && moveDirection.z <= 0.5)
        {
            // 0 < Lx <= 0.5 Walk
            Debug.Log("Walk");
            speed = 2;
        }
        else if (0.5 < moveDirection.z && moveDirection.z < 1)
        {
            // 0.5 < Lx < 1 Run
            Debug.Log("Run");
            speed = 5;
        }
        else if (moveDirection.z == 0) 
        {
            Debug.Log("Idle");
        }

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
    }
}
