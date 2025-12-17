using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float sprintMultiplier = 2f;
    PlayerInput playerInput;
    CharacterController controller;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float speed = movementSpeed;

        if (playerInput.actions["Sprint"].IsPressed())
        {
            speed = movementSpeed * sprintMultiplier;
        }

        controller.Move(speed * Time.deltaTime * CalculateMovement());
    }

    Vector3 CalculateMovement()
    {
        Vector2 inputValue = playerInput.actions["Move"].ReadValue<Vector2>();
        
        Vector3 right = (Camera.main.transform.right * inputValue.x).normalized;
        right.y = 0;

        Vector3 forward = (Camera.main.transform.forward * inputValue.y).normalized;
        forward.y = 0;

        return forward + right;
    }
}
