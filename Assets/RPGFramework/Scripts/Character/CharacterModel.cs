using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : CharacterRenderer
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private int index = -1;
    [SerializeField]
    private bool inited = false;

    private Vector2 target => ExplorerManager.instance.characterManager.Targets[index];

    private float playerSpeed => ExplorerManager.instance.playerManager.movement.Speed;
    private float accelerate => ExplorerManager.instance.playerManager.movement.IsRun ? 
                                    ExplorerManager.instance.playerManager.movement.AccelerationFactor : 1;

    private Vector2 direction = Vector2.zero;
    //private CommonDirection enumDirection;

    private void Start()
    {
        ExplorerManager.instance.playerManager.movement.OnMoving += Movement_OnMoving;
        ExplorerManager.instance.playerManager.movement.OnStopMoving += Movement_OnStopMoving;
        ExplorerManager.instance.playerManager.movement.OnRotate += Movement_OnRotate;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    public void Init(int index)
    {
        this.index = index;

        Rotate(ExplorerManager.GetPlayerViewDirection());

        inited = true;
    }

    private void Movement_OnRotate(CommonDirection obj)
    {
        if (index == 0 && inited)
        {
            Rotate(obj);
        }
    }

    private void Movement_OnStopMoving()
    {
        if (inited)
        {
            StopWalk();

            rb.velocity = new Vector2();
        }
    }

    private void Movement_OnMoving()
    {
        if (inited)
        {
            IsAccelerated = ExplorerManager.instance.playerManager.movement.IsRun;

            Vector2 newdirection = (target - (Vector2)transform.position).normalized;

            if (index == 0)
            {
                StartWalk();

                transform.position = target;
            }  
            else
            {
                if ((target - (Vector2)transform.position).magnitude > 0.1f)
                {
                    rb.velocity = direction * playerSpeed * accelerate;

                    StartWalk();
                }
                else
                {
                    rb.velocity = new Vector2();

                    StopWalk();
                }

                if (newdirection != direction)
                {
                    direction = newdirection;

                    CommonDirection newenum = CommonDirection.None;

                    if (direction.y > 0.5f)
                    {
                        newenum = CommonDirection.Up;
                    }
                    else if (direction.y < -0.5f)
                    {
                        newenum = CommonDirection.Down;
                    }
                    else if (direction.x > 0)
                    {
                        newenum = CommonDirection.Right;
                    }
                    else if (direction.x < 0)
                    {
                        newenum = CommonDirection.Left;
                    }


                    if (newenum != CurrentDirection)
                    {
                        Rotate(newenum);
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        ExplorerManager.instance.playerManager.movement.OnMoving -= Movement_OnMoving;
        ExplorerManager.instance.playerManager.movement.OnStopMoving -= Movement_OnStopMoving;
        ExplorerManager.instance.playerManager.movement.OnRotate -= Movement_OnRotate;
    }
}
