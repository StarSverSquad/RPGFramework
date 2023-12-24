using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCameraManager : MonoBehaviour
{
    public enum CameraLink
    {
        Player, LocationPoint, PlayerFollow, FreeMove
    }

    public enum MoveFuction
    {
        Linear, Interpolate
    }

    private CameraLink link = CameraLink.LocationPoint;
    public CameraLink Link => link;

    private bool isMoving = false;
    public bool IsMoving => isMoving;

    public Vector2 Offset;

    public float Speed;

    public float ZPosition = -5000;

    public MoveFuction MoveType = MoveFuction.Linear;

    [SerializeField]
    private Vector2 targetPoint = Vector2.zero;

    private void FixedUpdate()
    {
        UpdatePlayerFollow();
    }

    public void SetLocationPoint(LocationObject obj = null)
    {
        link = CameraLink.LocationPoint;

        LocationObject loc = obj ?? LocalManager.GetCurrentLocation();

        transform.position = new Vector3(loc.CameraPoint.position.x, loc.CameraPoint.position.y, transform.position.z);
    }

    public void SetPlayerFollow()
    {
        link = CameraLink.PlayerFollow;

        Vector2 playerPos = ExplorerManager.GetPlayerPosition();

        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }

    public void SetPlayer()
    {
        link = CameraLink.Player;

        Vector2 playerPos = ExplorerManager.GetPlayerPosition();

        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }

    private void UpdatePlayerFollow()
    {
        if (Link == CameraLink.PlayerFollow)
        {
            targetPoint = ExplorerManager.GetPlayerPosition();

            switch (ExplorerManager.GetPlayerViewDirection())
            {
                case CommonDirection.Up:
                    targetPoint += new Vector2(0, Offset.y);
                    break;
                case CommonDirection.Down:
                    targetPoint += new Vector2(0, -Offset.y);
                    break;
                case CommonDirection.Right:
                    targetPoint += new Vector2(Offset.x, 0);
                    break;
                case CommonDirection.Left:
                    targetPoint += new Vector2(-Offset.x, 0);
                    break;
                default:
                    Debug.LogError("Не возможно определить напровление взгляда игрока!");
                    break;
            }

            switch (MoveType)
            {
                case MoveFuction.Linear:
                    transform.position = Vector2.MoveTowards(transform.position, targetPoint, Speed * Time.fixedDeltaTime);
                    break;
                case MoveFuction.Interpolate:
                    transform.position = Vector2.Lerp(transform.position, targetPoint, Speed * Time.fixedDeltaTime);
                    break;
                default:
                    Debug.LogWarning("Такой функции нет!");
                    break;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, ZPosition);
        }
    }

    public void MoveTo()
    {
        /// TODO
    }

    private IEnumerator MoveToCoroutine()
    {
        isMoving = true;

        CameraLink prevLink = link;
        link = CameraLink.FreeMove;

        /// TODO

        link = prevLink;
        isMoving = false;

        yield break;
    }
}
