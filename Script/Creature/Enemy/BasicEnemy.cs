using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicEnemy : MonoBehaviour
{
    [Header("Enemy Health")]
    public float maxHealth = 20f;
    public float health;

    [Space]

    [Header("Enemy Movement")]
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float recoilx = 100f;
    [SerializeField] protected float recoily = 80f;
    [Space]

    [Header("Enemy Collision")]
    [SerializeField] protected LayerMask checkCollisionMask;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform fallCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float fallCheckRadius = 0.2f;
    [SerializeField] protected float wallCheckRadius = 0.2f;
    [SerializeField] protected float damageThroughContact = 10f;
    protected bool isGrounded;
    [Space]

    [Header("AI Control")]
    public bool aiIsActive = true;
    [Space]

    [Header("Audio")]
    [SerializeField] protected AudioClip attack;
    [SerializeField] protected AudioClip hit;

    [Header("Damage Particle")]
    [SerializeField] protected ParticleSystem damageSystem;
    
    protected AudioSource audioSource;
    protected Collider2D col;
    protected Rigidbody2D rb;
    protected Animator animator;

    [Header("Facing Direction")]
    public bool facingRight = true;
    protected bool isInvincible = false;
    protected bool isHitted;

    private void Start()
    {
        fallCheck = transform.Find("IsThereGroundCheck");
        wallCheck = transform.Find("WallCheck");
        groundCheck = transform.Find("IsGroundedCheck");

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        health = maxHealth;
    }

    private void Update()
    {
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        if(aiIsActive)
            Move();
    }

    #region AI Controls
    public virtual void Move()
    {
        if(health > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
        {
            bool thereIsGround = Physics2D.OverlapCircle(fallCheck.position, fallCheckRadius, checkCollisionMask); 
            bool thereIsWall  = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, checkCollisionMask);
            
            if(!isHitted && Mathf.Abs(rb.velocity.y) < 0.5f)
            {

                if (!isHitted && !thereIsWall && thereIsGround)
                {
                    if (facingRight)
                    {
                        rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, rb.velocity.y);
                    }
                }
                else if(!isHitted && (thereIsWall || !thereIsGround ))
                {
                    Flip();
                }
            }


        }
    }

    public void ActivateAI()
    {
        aiIsActive = true;
    }
    #endregion


    #region DamageEvents
    public virtual void TakeDamage(float damage)
    {
        if(!isInvincible && health > 0)
        {
            damageSystem.Play();
            animator.SetTrigger("Hurt");
            audioSource.clip = hit;
            audioSource.Play();

            float direction = damage / Mathf.Abs(damage);

            Debug.Log(direction);

            damage = Mathf.Abs(damage);

            health -= damage;

            if (health <= 0)
                StartCoroutine(Death());
            else
                StartCoroutine(Invincible());

            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direction * recoilx, recoily));

            if (facingRight && direction == 1)
                Flip();
            else if (!facingRight && direction == -1)
                Flip();

        }
    }

    protected virtual IEnumerator Invincible()
    {
        isInvincible = true;
        isHitted = true;
        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
        isHitted = false;
    }

    protected virtual IEnumerator Death()
    {
        isInvincible = true;
        gameObject.layer = LayerMask.NameToLayer("Dash");
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    #endregion

    #region Animation

    public virtual void HandleAnimation()
    {
        animator.SetFloat("MovementX", rb.velocity.x);
    }

    public void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }
    #endregion

    #region Collison Detection
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameObject player = collision.gameObject;
            if (player.transform.position.x - transform.position.x < 0)
            {
                player.gameObject.SendMessage("TakeDamage", damageThroughContact * -1f);
            }
            else
            {
                player.gameObject.SendMessage("TakeDamage", damageThroughContact);
            }
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (fallCheck != null)
            Gizmos.DrawWireSphere(fallCheck.position, fallCheckRadius);

        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
}
