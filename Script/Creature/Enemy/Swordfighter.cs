using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordfighter : Enemy
{
    [Header("Melee Stats")]
    [SerializeField] protected LayerMask layerToHurt;
    [SerializeField] protected Transform attackField;
    [SerializeField] protected float meleeAtk = 20f;
    [SerializeField] protected float attackRange = 0.4f;
    [SerializeField] protected float attackColliderRange = 0.2f;
    [SerializeField] protected float weaponTimer = 0.4f;
    protected float weaponTimerRemember = 0f;

    protected bool playAnimation = false;

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

        target = FindObjectOfType<PlayerCharacter>().gameObject;
        attackField = transform.Find("AttackField");
    }

    private void Update()
    {
        weaponTimerRemember -= Time.deltaTime;
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        EnemyBehavior();
    }

    public override void EnemyBehavior()
    {
        if(health > 0)
        {
            switch(enemyAggro)
            {
                case EnemyAggro.Combat:
                    if (Vector2.Distance(transform.position, target.transform.position) < attackRange && weaponTimerRemember < 0f && !isHitted)
                        MeeleAttack();
                    else if (!isHitted && playAnimation)
                        rb.velocity = new Vector2(0f, rb.velocity.y);
                    else if (Vector2.Distance(transform.position, target.transform.position) < attackRange && weaponTimerRemember > 0f && !isHitted)
                        MoveAwayFromTarget();
                    else if (!isHitted && !playAnimation)
                        MoveTowardTarget();
                    break;
                case EnemyAggro.Searching:
                    SearchTarget();
                    break;
                case EnemyAggro.NoCombat:
                    Move();
                    break;
            }
        }
    }

    protected virtual void MeeleAttack()
    {
        animator.SetTrigger("Attack");
        weaponTimerRemember = weaponTimer;
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    public void EnemyDealDamage()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackField.position, attackColliderRange, layerToHurt);

        foreach (Collider2D player in hitPlayer)
        {
            if((player.transform.position.x - transform.position.x) < 0)
                player.gameObject.SendMessage("TakeDamage", damageThroughContact * -1f);
            else
                player.gameObject.SendMessage("TakeDamage", damageThroughContact);
        }
    }

    public override void MoveTowardTarget()
    {
        bool thereIsGround = Physics2D.OverlapCircle(fallCheck.position, fallCheckRadius, checkCollisionMask);
        bool thereIsWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, checkCollisionMask);

        distToPlayer = target.transform.position.x - transform.position.x;
        distToPlayerY = target.transform.position.y - transform.position.y;

        if ((distToPlayer > 0 && !facingRight && isGrounded && !playAnimation) || (distToPlayer < 0 && facingRight && isGrounded && !playAnimation))
            Flip();

        if (!thereIsWall && thereIsGround && !isHitted && !playAnimation)
        {
            rb.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * aggroMovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (thereIsWall && thereIsGround && !playAnimation && !isHitted)
        {
            Jump();
        }
        else if (!thereIsWall && !thereIsGround && !playAnimation && !isHitted)
        {
            Jump();
        }
        else if (!isHitted && !isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        // Why did I created those two lines?
        //else if (!isHitted && playAnimation)
        //    rb.velocity = Vector2.zero;
    }

    public virtual void MoveAwayFromTarget()
    {
        rb.velocity = new Vector2((distToPlayer / Mathf.Abs(distToPlayer) * aggroMovementSpeed * Time.fixedDeltaTime) * -1f, rb.velocity.y);
    }

    public virtual void SearchTarget()
    {
        bool thereIsGround = Physics2D.OverlapCircle(fallCheck.position, fallCheckRadius, checkCollisionMask);
        bool thereIsWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, checkCollisionMask);

        distToPlayer = target.transform.position.x - transform.position.x;
        distToPlayerY = target.transform.position.y - transform.position.y;

        if ((distToPlayer > 0 && !facingRight) || (distToPlayer < 0 && facingRight))
            Flip();

        if (!thereIsWall && thereIsGround && !isHitted && !playAnimation)
        {
            rb.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * aggroMovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (thereIsWall && thereIsGround && !isHitted && !playAnimation)
        {
            Jump();
        }
        else if (!isHitted && !isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        else if (!isHitted)
            rb.velocity = Vector2.zero;
    }

    public virtual void Jump()
    {
        Vector2 targetVelocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * aggroMovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        Vector2 velocity = Vector2.zero;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
        if(isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, 4f /*jumpForce*/);
    }

    public void IsPlayingAnimation()
    {
        playAnimation = true;
    }

    public void StopPlayingAnimation()
    {
        playAnimation = false;
    }

    public override void HandleAnimation()
    {
        animator.SetBool("Grounded", isGrounded);
        base.HandleAnimation();
        animator.SetFloat("MovementY", rb.velocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (fallCheck != null)
            Gizmos.DrawWireSphere(fallCheck.position, fallCheckRadius);

        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, fallCheckRadius);

        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);

        if (attackField != null)
            Gizmos.DrawWireSphere(attackField.position, attackColliderRange);
    }
}
