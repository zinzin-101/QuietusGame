using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementScript : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] bool facingRight = true;
    public bool FacingRight => facingRight;

    private bool canMove;

    void Awake()
    {
        TryGetComponent(out rb);
    }

    void Start()
    {
        canMove = true;
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    public void Move(Vector2 input, float speed)
    {
        if (!canMove)
        {
            return;
        }

        Vector3 velocity = input;

        rb.velocity = Vector3.ClampMagnitude(velocity, 1f) * speed;

        if ((input.x > 0 && !facingRight) || (input.x < 0 && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 charScale = transform.localScale;
        charScale.x = charScale.x * -1;
        transform.localScale = charScale;
    }
    public void SetToggleMove(bool state)
    {
        canMove = state;
    }

    public void SetRigidbodyVelocity(Vector3 value)
    {
        rb.velocity = value;
    }

    public Vector3 GetCurrentVelocity()
    {
        return rb.velocity;
    }
}
