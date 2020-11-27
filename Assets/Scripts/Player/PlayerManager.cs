using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float speed, jumpHeight, fallMultiplier, lowJumpMultiplier, hangTime, jumpBufferLength, dashTime, dashSpeed;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer, enemyLayer;

    [SerializeField] ParticleSystem footsteps, impact;
    ParticleSystem.EmissionModule footEmission;

    float hangCounter = 0, dashCounter = 0;
    float jumpBufferCount;
    bool grounded, doubleJump, wasOnGround, jumpCD, dashing;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    public Direction facing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        footEmission = footsteps.emission;
    }

    private void FixedUpdate()
    {       
        Movement();
    }

    void Update()
    {
        Jump();
        JumpDown();
        Dash();
        Shoot();
    }

    void Movement()
    {
        //check ground colliders
        grounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);

        //move horizontal
        if(!dashing)
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime * 10, rb.velocity.y);

        //show footstep effect
        if(Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
        {
            footEmission.rateOverTime = 200f;
        }
        else
        {
            footEmission.rateOverTime = 0f;
        }      
    }

    void Jump()
    {
        // reset double jump
        if (grounded)
        {
            doubleJump = false;
        }
        
        // show impact effect
        if(!wasOnGround && grounded)
        {
            impact.gameObject.SetActive(true);
            impact.Stop();
            impact.Play();
        }

        // manage hangtime
        if (grounded)
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }
        
        // manage jumpBuffer
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCount = jumpBufferLength;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        // first jump
        if (jumpBufferCount >= 0 && hangCounter > 0f && !Input.GetKey(KeyCode.S))
        {

            if (jumpCD)  
                return;
      
            Invoke("JumpCooldown", 0.1f);

            rb.velocity = Vector2.up * jumpHeight;
           
            jumpCD = true;

            jumpBufferCount = 0;
        }

       // double jump
       if (Input.GetKeyDown(KeyCode.Space) && !grounded  && !doubleJump)
       {
           rb.velocity = Vector2.up * jumpHeight;
           doubleJump = true;
       }

        // elevate gravity when get to highest point of jumping
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // stop jumping higher when release jump button 
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // set/reset impact effect for next ground collision
        wasOnGround = grounded;
    }

    void JumpCooldown()
    {    
        jumpCD = false;
    }

    void JumpDown()
    {
        // jump down through platform
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            ManageBoxCollider();
            Invoke("ManageBoxCollider", 0.2f);
        }
    }

    void ManageBoxCollider()
    {
        boxCollider.enabled = !boxCollider.enabled;
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePos.Set(mousePos.x, mousePos.y, 0);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0, enemyLayer);

            if (hit)
            {
                Debug.Log("hit");
                Destroy(hit.transform.gameObject);
            }
        }
    }

    void Dash()
    {
        //check direction
        if(rb.velocity.x < 0)
        {
            facing = Direction.left;
        }
        else if(rb.velocity.x > 0)
        {
            facing = Direction.right;
        }

        //execute dashing
        if(facing == Direction.right && Input.GetKeyDown(KeyCode.LeftShift) && !dashing)
        {

            SetGravity();

            rb.gravityScale = 0;
            rb.velocity = new Vector2(dashSpeed * Time.deltaTime * 10, 0);

            Invoke("SetDash", dashTime);

            
        }
        else if (facing == Direction.left && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("left");
        }
    }

    void SetGravity()
    {
        dashing = !dashing;
        rb.gravityScale = 5;
    }

    public enum Direction
    {
        none,
        right,
        left
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
    }     
}
    

