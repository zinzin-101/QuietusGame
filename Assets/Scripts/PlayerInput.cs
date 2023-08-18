using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private MovementScript movementScript;
    private PickupScript pickupScript;

    Vector2 playerInput;

    //[SerializeField] AnimController animController;

    void Start()
    {
        TryGetComponent(out movementScript);
        TryGetComponent(out pickupScript);
    }

    void Update()
    {
        playerInput.y = Input.GetAxisRaw("Vertical");
        playerInput.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        movementScript.Move(playerInput.normalized);

        //animController.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }
}
