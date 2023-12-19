using System.Collections;
using UnityEngine;

public class DirectionalBullet : PatternBullet
{
    public Vector2 Direction = Vector2.zero;

    private void FixedUpdate()
    {
        transform.Translate(Direction * Time.fixedDeltaTime);
    }
}