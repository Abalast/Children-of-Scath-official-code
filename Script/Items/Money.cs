using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Collectables
{
    public float worth;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 thrust = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rb.velocity = thrust;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameInstance.gameInstance.player.soundEffects.AudioMoney();
            GameInstance.gameInstance.data.money += worth;
            GameInstance.gameInstance.playerUI.UpdateMoney();
            gameObject.SetActive(false);
        }
    }
}
