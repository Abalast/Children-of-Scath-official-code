using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player") || col.gameObject.layer == LayerMask.NameToLayer("Dash"))
        {
            col.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(col.gameObject);
        }
    }
}
