using UnityEngine;

public class RatController : MonoBehaviour
{
    // =========================================================
    // COMPONENTS
    // =========================================================

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sr;

    // =========================================================
    // TARGET
    // =========================================================

    [Header("Target")]
    public Transform player;

    // =========================================================
    // MOVEMENT
    // =========================================================

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectRange = 5f;
    public float stopDistance = 1.2f;
    float facing = 1f;
    bool isChasing;

    // =========================================================
    // UNITY
    // =========================================================

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Tìm player bằng tag
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        HandleDetect();

        HandleMove();

        HandleFlip();

        HandleAnimation();
    }

    // =========================================================
    // DETECT
    // =========================================================

    /// <summary>
    /// Kiểm tra player có vào tầm phát hiện không
    /// </summary>
    void HandleDetect()
    {
        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );
        // Dừng khi đủ gần player
        if (distance <= 1.5f)
        {
            rb.linearVelocity = new Vector2(
                0,
                rb.linearVelocity.y
            );

            return;
        }
        isChasing = distance <= detectRange;
    }

    // =========================================================
    // MOVE
    // =========================================================

    /// <summary>
    /// Di chuyển theo player
    /// </summary>
    void HandleMove()
    {
        // Nếu player ngoài range
        if (!isChasing)
        {
            rb.linearVelocity = new Vector2(
                0,
                rb.linearVelocity.y
            );

            return;
        }
        float distance =
    Vector2.Distance(
        transform.position,
        player.position
    );

        if (distance <= stopDistance)
        {
            rb.linearVelocity = new Vector2(
                0,
                rb.linearVelocity.y
            );

            return;
        }
        // Xác định hướng di chuyển
        float dir =
            player.position.x > transform.position.x
            ? 1f
            : -1f;
        facing = dir;
        Debug.Log(dir);
        // Move
        rb.MovePosition(
     rb.position +
     new Vector2(dir * moveSpeed * Time.fixedDeltaTime, 0)
         );
    }

    // =========================================================
    // FLIP
    // =========================================================

    /// <summary>
    /// Lật mặt rat theo vị trí player
    /// </summary>
    /// <summary>
    /// Lật sprite theo hướng nhìn
    /// </summary>
    void HandleFlip()
    {
        // Quay phải
        if (facing > 0)
        {
            sr.flipX = true;
        }
        // Quay trái
        else
        {
            sr.flipX = false;
        }
    }

    // =========================================================
    // ANIMATION
    // =========================================================

    /// <summary>
    /// Update animation movement
    /// </summary>
    void HandleAnimation()
    {
        anim.SetFloat(
            "Speed",
            Mathf.Abs(rb.linearVelocity.x)
        );
    }
}