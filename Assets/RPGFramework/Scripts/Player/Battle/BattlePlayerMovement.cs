using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattlePlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public bool CanMove = true;

    public float DefaultMoveSpeed = 1;
    public float MoveSpeed = 1;

    private void FixedUpdate()
    {
        if (CanMove)
        {
            Vector2 direction = Vector2.zero;

            if (Input.GetKey(GameManager.Instance.BaseOptions.MoveUp))
                direction += Vector2.up;
            if (Input.GetKey(GameManager.Instance.BaseOptions.MoveDown))
                direction += Vector2.down;
            if (Input.GetKey(GameManager.Instance.BaseOptions.MoveLeft))
                direction += Vector2.left;
            if (Input.GetKey(GameManager.Instance.BaseOptions.MoveRight))
                direction += Vector2.right;

            rb.linearVelocity = direction.normalized * MoveSpeed;
        }
        else
            rb.linearVelocity = Vector2.zero;
    }
}
