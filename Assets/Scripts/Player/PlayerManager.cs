using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float speed, jumpHeight, fallMultiplier, lowJumpMultiplier, hangTime, jumpBufferLength;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer, enemyLayer;

    [SerializeField] ParticleSystem footsteps, impact;
    ParticleSystem.EmissionModule footEmission;

    float hangCounter = 0;
    float jumpBufferCount;
    bool grounded;
    bool doubleJump;
    bool wasOnGround;
    bool jumpCD;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        footEmission = footsteps.emission;
    }

    private void FixedUpdate()
    {       
        Movement();
    }

    void Update()
    {
        Jump();
        Shoot();
    }

    void Movement()
    {
        //check ground colliders
        grounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);

        //move horizontal
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime * 10, rb.velocity.y);

        //show footstep effect
        if(Input.GetAxisRaw("Horizontal") != 0)
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCount = jumpBufferLength;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        // first jump
        if (jumpBufferCount >= 0 && hangCounter > 0f)
        {

            if (jumpCD)  
                return;
      
            StartCoroutine(JumpCooldown());

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

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        jumpCD = false;
    }

    void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePos.Set(mousePos.x, mousePos.y, 0);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0, enemyLayer);
                
            if(hit)
            {
                Debug.Log("hit");
                Destroy(hit.transform.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
    }
}
    

