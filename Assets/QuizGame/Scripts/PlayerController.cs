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
    
    private float moveHorizontal = 2f;
    private float jumpPower = 2f;
    private float jumpDuration = 0.5f;
    
    private bool canMove = true;

    private void Update()
    {
        Movement();
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
}
