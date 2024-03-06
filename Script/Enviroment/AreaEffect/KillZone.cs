using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    public float damage = 50f;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player") || col.gameObject.layer == LayerMask.NameToLayer("Dash"))
        {
            col.gameObject.GetComponent<PlayerStats>().TakeDamage(damage, true);
        }
        else if(col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(col.gameObject);
        }
    }
}
