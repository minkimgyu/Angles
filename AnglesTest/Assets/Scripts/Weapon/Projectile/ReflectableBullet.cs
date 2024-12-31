using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectableBullet : Bullet
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);

        Vector2 nomal = collision.contacts[0].normal;
        Vector2 direction = Vector2.Reflect(transform.right, nomal);
        Shoot(direction, _force);
    }
}
