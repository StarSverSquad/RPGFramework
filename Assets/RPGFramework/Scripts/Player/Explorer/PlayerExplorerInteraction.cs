using UnityEngine;

public class PlayerExplorerInteraction : MonoBehaviour
{
    public enum ViewDirection
    {
        None, Up, Down, Right, Left
    }

    public Vector2 Direction = Vector2.down;

    public float InteractionDistance = 2.5f;

    public bool CanInteract = true;

    [SerializeField]
    private Collider2D raycastHit;

    private void FixedUpdate()
    {
        if (ExplorerManager.Instance.PlayerManager.movement.CanWalk 
            && !ExplorerManager.Instance.EventHandler.EventRuning)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Direction = new Vector2(1, 0);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Direction = new Vector2(-1, 0);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Direction = new Vector2(0, 1);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                Direction = new Vector2(0, -1);
            }
        }
    }

    private void Update()
    {
        raycastHit = Physics2D.Raycast(transform.position, Direction, InteractionDistance, LayerMask.GetMask("EventInteraction")).collider;

        ExplorerEvent @event = raycastHit != null ? raycastHit.gameObject.GetComponent<ExplorerEvent>() : null;

        if (@event != null)
        {
            if (Input.GetKeyDown(KeyCode.Z) && CanInteract && @event.Interaction == ExplorerEvent.InteractionType.OnClick)
            {
                @event.InvokeEvent();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ExplorerEvent @event = collision.gameObject.GetComponent<ExplorerEvent>();

        if (@event != null)
        {
            if (@event.Interaction == ExplorerEvent.InteractionType.OnStep && CanInteract)
            {
                @event.InvokeEvent();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ExplorerEvent @event = collision.gameObject.GetComponent<ExplorerEvent>();

        if (@event != null)
        {
            if (Input.GetKeyDown(KeyCode.Z) && CanInteract && @event.Interaction == ExplorerEvent.InteractionType.OnClick)
            {
                @event.InvokeEvent();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Direction * InteractionDistance, Color.red);
    }
}
