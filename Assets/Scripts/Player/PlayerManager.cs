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
    bool grounded, doubleJump, wasOnGround, jumpCD, dashing, dashCD;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    Collider2D groundCollider;

    public Direction facing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        footEmission = footsteps.emission;     
    }

    void FixedUpdate()
    {       
        Movement();   
    }

    void Update()
    {
        Jump();
        JumpDown();
        PrepareDash();
        Shoot();   
    }

    void Movement()
    {
        if(!dashing)
        {
            //check ground colliders and set bool to true
            groundCollider = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
            grounded = groundCollider;
     
            //move horizontal
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime * 10, rb.velocity.y);

            //reset dash when hit the ground
            if (grounded)
                dashCD = false;
        }

        //show footstep effect
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
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

        // show impact when hit the ground
        if (!wasOnGround && grounded)
        {
            ImpactEffect();
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

        // jump
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
           ImpactEffect();
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

    // avoid trtiple jump
    void JumpCooldown()
    {    
        jumpCD = false;
    }

    void JumpDown()
    {
        // jump down through platform
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && grounded && groundCollider.gameObject.name != "BaseGround" )
        {
            ManageBoxCollider();
            Invoke("ManageBoxCollider", 0.15f);
        }
    }

    //disable box collider for jumping down trough platform
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

    void PrepareDash()
    {
        //set direction
        if(rb.velocity.x < 0)
        {
            facing = Direction.left;
        }
        else if(rb.velocity.x > 0)
        {
            facing = Direction.right;
        }

        //execute right dash
        if(facing == Direction.right && Input.GetKeyDown(KeyCode.LeftShift) && !dashing && !dashCD)
        { 
            Dash(dashSpeed);
        }
        //execute left dash
        else if (facing == Direction.left && Input.GetKeyDown(KeyCode.LeftShift) && !dashing && !dashCD )
        {
            Dash(-dashSpeed);
        }
    }

    // execute dashing
    void Dash(float speed)
    {
        ImpactEffect();

        SetGravity();

        rb.velocity = new Vector2(speed * Time.deltaTime * 10, 0);

        Invoke("SetGravity", dashTime);

        dashCD = true;
    }

    // set gravity to 0 while dashing
    void SetGravity()
    {
        dashing = !dashing;

        if(rb.gravityScale == 5)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 5;
        }
    }

    void ImpactEffect()
    {
        // show impact effect
        
        impact.gameObject.SetActive(true);
        impact.Stop();
        impact.Play();
        
    }

    public enum Direction
    {
        none,
        right,
        left
    }

    //Show groundcheckbox
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
    }     
}
    

