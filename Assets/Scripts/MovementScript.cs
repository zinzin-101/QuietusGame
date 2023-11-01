using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementScript : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] bool facingRight = true;
    public bool FacingRight => facingRight;

    private bool canMove;
    private bool isMoving;

    [SerializeField] float moveSpeed = 5f;

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
        //
        //canMove = GameManager.Instance.PlayerCanMove;
        //
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    public void Move(Vector2 input)
    {

        if (!canMove)
        {
            return;
        }

        if (input != Vector2.zero)
        {
            rb.velocity = input * moveSpeed;
            SoundManager.PlaySound(SoundManager.Sound.PlayerMove);

        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        

        if ((input.x > 0 && !facingRight) || (input.x < 0 && facingRight))
        {
            //Flip();
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
