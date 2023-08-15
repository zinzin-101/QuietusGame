using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private MovementScript movementScript;
    [SerializeField] float moveSpeed = 10f;

    Vector2 playerInput;

    //[SerializeField] AnimController animController;

    void Start()
    {
        TryGetComponent(out movementScript);
    }

    void Update()
    {
        playerInput.y = Input.GetAxisRaw("Vertical");
        playerInput.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        movementScript.Move(playerInput, moveSpeed);

        //animController.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }
}
