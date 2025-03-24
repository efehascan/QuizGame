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
    [SerializeField] private Transform[] deathTargets;
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


    
    
    /// <summary>
    /// Karakterin tüm fiziksel hareketlerini dondurarak hareket etmesini engeller.
    /// </summary>
    public void UnFreezePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    
    /// <summary>
    /// Karakterin tüm fiziksel hareketlerini dondurarak hareket etmesini engeller.
    /// </summary>
    private void FreezePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }


    
    /// <summary>
    /// Oyuncunun yatay (horizontal) girişine göre karakteri hareket ettirir.
    /// Rigidbody2D bileşeninin yatay hızını giriş yönünde güncellerken, dikey hızını değiştirmez.
    /// </summary>
    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(horizontalInput * moveHorizontal, rb.velocity.y);
        rb.velocity = move;
    }

    
    /// <summary>
    /// Karakterin belirlenen hedef noktalar boyunca belirtilen sürede hareket etmesini sağlar.
    /// Hareket başladığında oyuncunun kontrolünü kapatır ve hareket tamamlandığında tekrar açarak oyuncuyu serbest bırakır.
    /// DOTween kütüphanesi kullanılarak animasyon gerçekleştirilir.
    /// </summary>
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
    
    
    /// <summary>
    /// Karakterin ölüm anında belirlenmiş hedef pozisyonlar boyunca hareket etmesini sağlar.
    /// Hareket başlamadan oyuncunun kontrolünü kapatır ve hareket tamamlandıktan sonra tekrar açar.
    /// DOTween animasyonu kullanılarak doğrusal bir yol izlenir.
    /// </summary>
    public void DeathPath()
    {
        var positions = deathTargets.Select(d => d.position).ToArray();
        transform.DOPath(positions, pathDuration, PathType.Linear)
            .SetEase(pathEase)
            .SetOptions(false)
            .OnStart(() => {
                canMove = false;
            }).OnComplete(() => {
                canMove = true;
            });
    }

    
    /// <summary>
    /// Karaktere dikey yönde, belirtilen zıplama gücüyle anlık hız kazandırarak zıplamasını sağlar.
    /// </summary>
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    
    /// <summary>
    /// Karakterin zemine temas edip etmediğini kontrol eder.
    /// Zemine temas durumunu "isGrounded" değişkenine atar.
    /// Kontrol, belirtilen noktada ve yarıçapta fizik çakışmasıyla yapılır.
    /// </summary>
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
