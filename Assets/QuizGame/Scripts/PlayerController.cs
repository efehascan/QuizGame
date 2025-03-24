using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion
    
    
    [SerializeField] Rigidbody2D rb;
    
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = 0.2f;
    
    private float moveHorizontal = 5f;
    private float jumpPower = 5f;
    
    private bool canMove = true;
    private bool isGrounded;

    private void Update()
    {
        Movement();
        CheckGround();
        
        if(Input.GetKeyDown(KeyCode.Space) && canMove && isGrounded)
            Jump();
        
        
    }


    private void FreezePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(horizontalInput * moveHorizontal, rb.velocity.y);
        rb.velocity = move;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
