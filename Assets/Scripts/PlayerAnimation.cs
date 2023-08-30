using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    public string[] IdleDirection = { "Char_N_Idle", "Char_NW_Idle", "Char_W_Idle", "Char_SW_Idle", "Char_S_Idle", "Char_SE_Idle", "Char_E_Idle", "Char_NE_Idle" };
    public string[] WalkDirection = { "Char_N", "Char_NW", "Char_W", "Char_SW", "Char_S", "Char_SE", "Char_E", "Char_NE" };

    int lastDirection;
    private void Awake()
    {
        anim = GetComponent<Animator>();

        float result1 = Vector2.SignedAngle(Vector2.up, Vector2.right);
    }
    public void SetDirection(Vector2 _direction)
    {
        string[] directionArray = null;

        if (_direction.magnitude < 0.01)
        {
            directionArray = IdleDirection;
        }
        else
        {
            directionArray = WalkDirection;

            lastDirection = DirectionToIndex(_direction);
        }

        anim.Play(directionArray[lastDirection]);
    }

    private int DirectionToIndex(Vector2 _direction)
    {
        Vector2 norDir = _direction.normalized;

        float step = 360 / 8;
        float offset = step / 2;

        float angle = Vector2.SignedAngle(Vector2.up, norDir);

        angle += offset;
        if (angle < 0)
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }
}

