using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [Header("Spear Stats")]
    public float attackRange;
    public float weaponDamage;
    public float speed = 300f;
    [Space]

    [Header("LayerMask")]
    public LayerMask enemyMask;
    public LayerMask enviromentMask;
    [Space]

    Rigidbody2D rb;
    Collider2D col;

    [Header("Debuging purpose")]
    public Transform playerTransform;

    [HideInInspector] public Vector2 direction;
    [HideInInspector] public GameObject User;

    FollowCamera cam;

    bool continueFlying = true;
    bool returnToWielder = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        cam = FindObjectOfType<FollowCamera>();
    }

    private void FixedUpdate()
    {
        Fly();
    }

    public void Fly()
    {
        if (continueFlying && !returnToWielder)
            rb.velocity = direction * (speed * Time.fixedDeltaTime);
        else if(continueFlying)
        {
            transform.position = Vector2.MoveTowards(transform.position, User.transform.position, 10f * Time.fixedDeltaTime);

            if(Vector2.Distance(transform.position, User.transform.position) < 0.4f)
            {
                ReturnedToWielder();
            }
        }
    }

    public void ReturnToWielder()
    {
        UnFreezBody();
        rb.velocity = Vector2.zero;
        returnToWielder = true;
        continueFlying = true;
    }
    public void ReturnedToWielder()
    {
        Debug.Log("Got Back");
        User.GetComponent<PlayerAction>().possessSpear = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {     
        if (collision.gameObject.IsInLayerMasks(enemyMask) && !returnToWielder)
        {
            if (collision.transform.position.x - transform.position.x < 0)
            {
                collision.gameObject.SendMessage("TakeDamage", weaponDamage * -1f);
            }
            else
            {
                collision.gameObject.SendMessage("TakeDamage", weaponDamage);
            }
            cam.GetComponent<FollowCamera>().ShakeCamera();
        }

        if(collision.gameObject.IsInLayerMasks(enviromentMask) && !returnToWielder)
        {
            FreezBody();
        }
            
    }

    void FreezBody()
    {
        continueFlying = false;
        gameObject.layer = LayerMask.NameToLayer("Ground");
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        col.isTrigger = false;

    }

    void UnFreezBody()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        rb.constraints = RigidbodyConstraints2D.None;
        col.isTrigger = true;
    }
}
