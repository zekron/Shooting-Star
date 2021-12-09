using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderDetetor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            projectile.gameObject.SetActive(false);
        }
        else if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
