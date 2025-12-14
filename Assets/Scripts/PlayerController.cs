using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    PlayerInput playerInput;
    CharacterController controller;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        controller.Move(movementSpeed * Time.deltaTime * CalculateMovement());
    }

    Vector3 CalculateMovement()
    {
        Vector2 inputValue = playerInput.actions["Move"].ReadValue<Vector2>();
        return new Vector3(inputValue.x, 0f, inputValue.y);
    }
}
