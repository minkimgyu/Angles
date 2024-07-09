using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaReflecter : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IProjectile projectile = collision.gameObject.GetComponent<IProjectile>();
        if (projectile == null) return;

        Vector2 directionVec = projectile.ReturnDirectionVector();

        Vector2 direction = Vector2.Reflect(directionVec, collision.contacts[0].normal);
        projectile.Shoot(direction);
    }
}
