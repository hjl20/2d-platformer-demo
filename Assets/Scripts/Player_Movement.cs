
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalMove;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isJumping;
    
    private bool isWallJumping;
    private bool isWallSliding;
    private float availableJumpTime;
    RaycastHit2D wallCheckHit;

    [Header("Horizontal Movement")]
    public float speed = 1f;

    [Header("Vertical Movement")]
    public float jumpForce = 125f;

    [Header("Bounds Checks")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.01f;

    [Header("Wall Jump")]
    public float extraJumpTime = 0.05f;
    public float wallSlideSpeed = 0.1f;
    public float wallDistance = 0.09f;


    //Initialize game objects
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //Updates every frame
    private void Update()
    {
        //Get horizontal input and animate player movement
        horizontalMove = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        //Flip sprite when moving in opposite direction
        if (horizontalMove > 0 && !isFacingRight)
        {
            FlipCharacter();
        } else if (horizontalMove < 0 && isFacingRight)
        {
            FlipCharacter();
        }

        //Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        
        //Grounded jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        //Wall check (checks BEHIND player for wall)
        if (isFacingRight) {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
            //Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.blue);
        } else if (!isFacingRight) {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
            //Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.blue);
        }
        //Wall slide
        if (wallCheckHit && !isGrounded && horizontalMove != 0) {
            isWallSliding = true;

            //More time for player to react for jump
            availableJumpTime = Time.time + extraJumpTime;

            //Wall jump
            if (Input.GetButtonDown("Jump")) {
                isWallJumping = true;
            }

        } else if (Time.time > availableJumpTime) {
            isWallSliding = false;
        }
    }

    //Updates at specific intervals for physics
    private void FixedUpdate()
    {
        //Set player's horizontal velocity
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        //Grounded
        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }

        //Jumping
        if (isJumping)
        {
            Jump();
        }
        isJumping = false;

        //Wall slide
        if (isWallSliding) {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }

        //Wall jump
        if (isWallJumping) {
            Jump();
        }
        isWallJumping = false;

    }

    private void Jump()
    {
        animator.SetBool("IsJumping", true);
        rb.AddForce(new Vector2(0f, jumpForce));
    }

    //Flips sprite
    private void FlipCharacter()
    {
        //Inverse bool check
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

}


