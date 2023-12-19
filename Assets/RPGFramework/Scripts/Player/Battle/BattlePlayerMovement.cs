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

            if (Input.GetKey(KeyCode.UpArrow))
                direction += Vector2.up;
            if (Input.GetKey(KeyCode.DownArrow))
                direction += Vector2.down;
            if (Input.GetKey(KeyCode.LeftArrow))
                direction += Vector2.left;
            if (Input.GetKey(KeyCode.RightArrow))
                direction += Vector2.right;

            rb.velocity = direction.normalized * MoveSpeed;
        }
        else
            rb.velocity = Vector2.zero;
    }
}
