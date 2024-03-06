using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStones : Trap
{
    public float damage;
    public float gravityScale = 4f;

    public LayerMask toDamage;
    public ParticleSystem dust;

    public Rigidbody2D rb;
    public Collider2D col;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        col.enabled = false;
    }

    public override void ActivateTrap()
    {
        StartCoroutine(StartFalling());
    }

    IEnumerator StartFalling()
    {
        col.enabled = true;
        dust.Play();
        yield return new WaitForSeconds(0.3f);
        dust.Play();
        col.enabled = true;
        rb.gravityScale = 4f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.IsInLayerMasks(toDamage))
        {
            collision.gameObject.SendMessage("TakeDamage", damage);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            dust.Play();
            col.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
