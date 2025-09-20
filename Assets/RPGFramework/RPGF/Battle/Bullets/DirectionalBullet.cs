using RPGF.Battle.Pattern;
using System.Collections;
using UnityEngine;

public class DirectionalBullet : PatternBulletBase
{
    [SerializeField]
    private GameObject model;
    public GameObject Model => model;

    public Vector2 Direction = Vector2.zero;

    private void FixedUpdate()
    {
        transform.Translate(Direction * Time.fixedDeltaTime);
    }
}