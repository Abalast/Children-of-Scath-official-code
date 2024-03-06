using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AnimationState { Idle, Combat_Idle, Run, Fall, Landed, Attack, Slide, Dash, Death}
public enum LocomotionState { Ground, Air}

public class PlayerLocomotion : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed = 200f;
    [SerializeField] float teleportSpeed = 20f;
    [SerializeField] float movementSmothing = 0.05f;
    [SerializeField]
    [Range(0f, 2f)] float airControl = 0.8f;
    [SerializeField] PhysicsMaterial2D moving;
    [SerializeField] PhysicsMaterial2D notMoving;
    [Space]

    [Header("Dash")]
    [SerializeField] float dashForce = 200f;
    [SerializeField] float dashTime = 0.1f;
    [SerializeField] float dashCooldown = 0.5f;
    [Space]

    [Header("Jump")]
    [SerializeField] float jumpForce = 220f;
    [SerializeField] float vaultJumpForce = 5.5f;
    [SerializeField] float jumpRememberTime = 0.2f;
    float jumpRemember = 0f;                           // Please Do not Change this variable
    [SerializeField] float groundedRememberTime = 0.2f;
    float groundRemember = 0f;                         // Please Do not Change this variable
    [SerializeField]
    [Range(0f,1f)] float cutJumpHeight = 0.5f;
    [SerializeField]
    [Range(1f, 5f)] float gravityScaleMul = 1.5f;
    float gravityScale;
    [Space]

    [Header("WallJump")]
    [SerializeField] float wallSlideSpeed = -0.3f;
    [SerializeField] bool wallCheckHit;
    [SerializeField] public bool isWallSliding;
    [Space]

    [Header("Movement Particle System")]
    [SerializeField] ParticleSystem smokeGround;
    [SerializeField] ParticleSystem smokeTrail;
    [SerializeField] ParticleSystem dashTrail;
    [SerializeField] ParticleSystem teleportTrail;
    [Space]

    [Header("Collision Checks")]
    [SerializeField] LayerMask whatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] Transform groundCheckCollision;                  // A position marking where to check if the player is grounded.
    [SerializeField] Transform wallCheckCollision;                    // A position marking where to check if the player on walls.
    [SerializeField] const float groundedRadius = .04f;
    [SerializeField] const float wallRadius = .04f;
    [SerializeField] public bool isGrounded = true;
    [Space]

    [Header("Debuging")]
    [Space]

    [Header("DashBool only seeable for Debuging")]
    [SerializeField] bool canDash = true;
    [SerializeField] public bool isDashing;
    [SerializeField] public bool isTeleporting;
    [SerializeField] public bool facingRight = true;
    [SerializeField] public GameObject spear;
    [Space]

    // Components
    [HideInInspector] public Rigidbody2D rigidBody;
    FollowCamera cam;
    SoundEffects soundEffects;
    PlayerCharacter character;

    // Movement reference
    Vector2 velocity = Vector2.zero;

    //
    // Function Start
    //

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        soundEffects = GetComponentInChildren<SoundEffects>();
        cam = FindObjectOfType<FollowCamera>();

        character = GetComponent<PlayerCharacter>();

        groundRemember = groundedRememberTime;
        gravityScale = rigidBody.gravityScale;
    }

    public void Update()
    {
        jumpRemember -= Time.deltaTime;
        if(!isGrounded && !isWallSliding)
            groundRemember -= Time.deltaTime;

        if(isWallSliding)
            cam.ChangeLookAheadDirection(!facingRight);
        else
            cam.ChangeLookAheadDirection(facingRight);
    }

    public void Move(float inputX, float inputY)
    {
        #region Flip()

        if (inputX > 0 && !facingRight)
        {
            Flip();
        }
        else if (inputX < 0 && facingRight)
        {
            Flip();
        }
        #endregion

        #region Movement()

        if (inputX != 0 || isDashing || !isGrounded)
            rigidBody.sharedMaterial = moving;
        else
            rigidBody.sharedMaterial = notMoving;

        //Code for Teleporting movement
        if (isTeleporting)
        {
            if(Vector2.Distance(gameObject.transform.position, spear.transform.position) <= 0.1f)
            {
                rigidBody.gravityScale = 1;
                isTeleporting = false;
                rigidBody.velocity = Vector2.zero;
                gameObject.layer = LayerMask.NameToLayer("Player");
                gameObject.transform.position = spear.GetComponent<Spear>().playerTransform.position;

            }
            else
            {
                if ((transform.position.x - spear.transform.position.x) < 0 && !facingRight)
                {
                    Flip();
                }
                else if ((transform.position.x - spear.transform.position.x) > 0 && facingRight)
                {
                    Flip();
                }

                dashTrail.Play();
                transform.position = Vector2.MoveTowards(transform.position, spear.transform.position, teleportSpeed * Time.fixedDeltaTime);    
            }
        }
        else if ((isDashing && isGrounded && !character.canDashMidAir) || (isDashing && character.canDashMidAir)) // code for Dashing without Wallsliding
        {
            if (facingRight)
                rigidBody.velocity = Vector2.right * dashForce;
            else
                rigidBody.velocity = Vector2.left * dashForce;

            if (!isGrounded)
                dashTrail.Play();
            else
                smokeTrail.Play();
        }
        else if (isWallSliding && !isGrounded)
        {
            rigidBody.velocity = new Vector2(0, Mathf.Clamp(rigidBody.velocity.y, wallSlideSpeed, float.MaxValue));
            smokeTrail.Play();
        }
        else if (!isGrounded && !isWallSliding) // code for movement while in the air.
        {
            Vector2 targetVelocity = new Vector2(inputX * movementSpeed * airControl, rigidBody.velocity.y);
            rigidBody.velocity = Vector2.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, movementSmothing);
        }
        else if (isGrounded) // code for ground movement
        {
            Vector2 targetVelocity = new Vector2(inputX * movementSpeed, rigidBody.velocity.y);
            rigidBody.velocity = Vector2.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, movementSmothing);

            if (inputX != 0)
                smokeTrail.Play();
        }
        #endregion

        #region Jump
        if ((jumpRemember > 0) && groundRemember > 0)
        {
            soundEffects.AudioJump();

            groundRemember = 0;
            jumpRemember = 0;



            // Jump function while sliding wall
            if (isWallSliding && !isGrounded)
            {
                if(facingRight)
                {

                    Flip();
                    rigidBody.velocity = new Vector2(-jumpForce * 1.5f, jumpForce);
                }
                else
                {

                    Flip();
                    rigidBody.velocity = new Vector2(jumpForce * 1.5f, jumpForce);
                }
            }
            else if (rigidBody.velocity.x != 0 && inputY < 0f && character.playerAction.possessSpear)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, vaultJumpForce);
            }
            else // Jump for anything else
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            }
            smokeGround.Play();
            smokeTrail.Play();
            isWallSliding = false;
        }
        #endregion

        #region gravityScale
        if (rigidBody.velocity.y < -0.2f)
            rigidBody.gravityScale = gravityScale * gravityScaleMul;
        else
            rigidBody.gravityScale = gravityScale;
        #endregion
    }

    #region Ground and Wall Check()
    public void GroundAndWallCheck()
    {
        #region Wall Check
        wallCheckHit = Physics2D.OverlapCircle(wallCheckCollision.position, wallRadius, whatIsGround);

        if (wallCheckHit && character.canWallJump)
        {
            isWallSliding = true;
            groundRemember = groundedRememberTime;
        }
        else if (!wallCheckHit)
        {
            isWallSliding = false;
        }

        #endregion

        #region Ground Check
        isGrounded = false;
        
        bool touchingGround = Physics2D.OverlapCircle(groundCheckCollision.position, groundedRadius, whatIsGround);

        if (touchingGround)
        {
            isGrounded = true;
            groundRemember = groundedRememberTime;
        }
        else
        {
            isGrounded = false;
        }
        #endregion
    }
    #endregion

    #region Jump Function
    public void Jump()
    {
        jumpRemember = jumpRememberTime;
    }

    public void CancelJump()
    {
        if (rigidBody.velocity.y > 0)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * cutJumpHeight);
    }
    #endregion

    #region Dash Function
    public void Dash()
    {
        if (canDash && isDashing == false)
            StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        if(isWallSliding)
            Flip();

        isDashing = true;
        canDash = false;
        //gameObject.layer = LayerMask.NameToLayer("Dash");
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        //gameObject.layer = LayerMask.NameToLayer("Player");
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public bool CanDash()
    {
        return canDash;
    }

    #endregion

    protected void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        if(groundCheckCollision != null)
            Gizmos.DrawWireSphere(groundCheckCollision.position, groundedRadius);

        if(wallCheckCollision != null)
            Gizmos.DrawWireSphere(wallCheckCollision.position, wallRadius);
    }
}
