using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private MovementScript movementScript;
    [SerializeField] PlayerAnimation playerAnimation;

    Vector2 playerInput;

    [SerializeField] KeyCode inventoryKey = KeyCode.Tab;

    //[SerializeField] AnimController animController;

    void Start()
    {
        TryGetComponent(out movementScript);
    }

    void Update()
    {
        playerInput.y = Input.GetAxisRaw("Vertical");
        playerInput.x = Input.GetAxisRaw("Horizontal");

        playerAnimation.SetDirection(playerInput);

        if (Input.GetKeyDown(inventoryKey))
        {
            InventoryManager.Instance.ToggleInventory();
        }
    }

    private void FixedUpdate()
    {
        movementScript.Move(playerInput.normalized);

        //animController.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }
}
