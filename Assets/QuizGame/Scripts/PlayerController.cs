using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Singleton { get; private set; }

    private void Awake()
    {
        _startPosition = transform.position;
        
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
    [SerializeField] Transform pathPoint;
    public const string TriggerTag = "Trigger";
    
    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = 0.2f;
    
    [Header("DOTween Move")]
    [SerializeField] private float pathDuration = 1f;
    [SerializeField] private Transform[] targets;
    [SerializeField] private Ease pathEase = Ease.Linear;
    
    private Vector2 _startPosition;
   
    private float moveHorizontal = 5f;
    private float jumpPower = 5f;
    
    private bool canMove = true;
    private bool isGrounded;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
        CheckGround();
        
        if(Input.GetKeyDown(KeyCode.Space) && canMove && isGrounded)
            Jump();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TriggerTag))
        {
            FreezePlayer();
            MovePointManager.Singleton.MovePoint(pathPoint);
            QuestionManager.Singleton.AskRandomQuestion();
        }
    }


    public void UnFreezePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    private void FreezePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }


    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(horizontalInput * moveHorizontal, rb.velocity.y);
        rb.velocity = move;
    }

    public void FollowPath()
    {
        var positions = targets.Select(t => t.position).ToArray();
        transform.DOPath(positions, pathDuration, PathType.Linear)
            .SetEase(pathEase)
            .SetOptions(false)
            .OnStart(() => {
                canMove = false;
            }).OnComplete(() => {
                canMove = true;
                UnFreezePlayer();
            });
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
