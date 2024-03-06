using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAggro { NoCombat, Searching, Combat }


public class Enemy : BasicEnemy
{
    [Header("Aggro")]
    [SerializeField] public bool aggro;
    [SerializeField] protected float searchingTime;
    [SerializeField] protected float aggroMovementSpeed = 5f;
    [SerializeField] protected EnemyAggro enemyAggro = EnemyAggro.NoCombat;

    
    protected GameObject target;
    protected EnemyFieldOfView FOV;

    protected float distToPlayer;
    protected float distToPlayerY;

    private void Start()
    {
        fallCheck = transform.Find("IsThereGroundCheck");
        wallCheck = transform.Find("WallCheck");
        groundCheck = transform.Find("IsGroundedCheck");

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        FOV = GetComponent<EnemyFieldOfView>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        health = maxHealth;

        target = FindObjectOfType<PlayerCharacter>().gameObject;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        if (aiIsActive)
            EnemyBehavior();
    }

    public virtual void EnemyBehavior()
    {
        if(health > 0)
        {
            switch(enemyAggro)
            {
                case EnemyAggro.NoCombat:
                    Move();
                    break;
                case EnemyAggro.Searching:
                    MoveTowardTarget();
                    break;
                case EnemyAggro.Combat:
                    MoveTowardTarget();
                    break;
            }
        }
    }

    public virtual void MoveTowardTarget()
    {
        bool thereIsGround = Physics2D.OverlapCircle(fallCheck.position, fallCheckRadius, checkCollisionMask);
        bool thereIsWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, checkCollisionMask);

        distToPlayer = target.transform.position.x - transform.position.x;
        distToPlayerY = target.transform.position.y - transform.position.y;

        if ((distToPlayer > 0 && !facingRight) || (distToPlayer < 0 && facingRight))
            Flip();

        if (!thereIsWall && thereIsGround && !isHitted)
        {
            rb.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * aggroMovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (!isHitted && !isGrounded)
            rb.velocity = new Vector2(0f, rb.velocity.y);
        else if (!isHitted)
            rb.velocity = Vector2.zero;

    }

    public virtual void Aggro()
    {
        enemyAggro = EnemyAggro.Combat;
    }

    public virtual void IsSearching()
    {
        if(enemyAggro == EnemyAggro.Combat)
            StartCoroutine(Searching());
    }

    protected virtual IEnumerator Searching()
    {
        enemyAggro = EnemyAggro.Searching;
        yield return new WaitForSeconds(searchingTime);
        if (enemyAggro == EnemyAggro.Searching)
            enemyAggro = EnemyAggro.NoCombat;
    }

    public virtual void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, fallCheckRadius, whatIsGround);
    }
}
