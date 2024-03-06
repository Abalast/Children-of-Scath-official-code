using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBullet : MonoBehaviour
{
	public Vector2 direction;
	public LayerMask hitLayerMask;
	public float damage = 20f;
	public float speed = 15f;
	public GameObject owner;

	// Update is called once per frame
	void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = direction * speed * Time.fixedDeltaTime;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			GameObject player = collision.gameObject;
			if (player.transform.position.x - transform.position.x < 0)
			{
				player.gameObject.SendMessage("TakeDamage", damage * -1f);
			}
			else
			{
				player.gameObject.SendMessage("TakeDamage", damage);
			}

			Destroy(gameObject);
		}
		else if(collision.gameObject.IsInLayerMasks(hitLayerMask))
		{
			Destroy(gameObject);
		}
	}
}
