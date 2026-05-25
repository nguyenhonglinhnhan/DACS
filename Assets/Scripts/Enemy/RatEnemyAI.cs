using UnityEngine;

public class RatEnemyAI : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Chase,
        Attack,
        Hurt,
        Dead
    }

    [Header("Move Settings")]
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public float chaseSpeedMultiplier = 1.5f;

    [Header("Detection")]
    public float detectRange = 5f;
    public float attackRange = 1.2f;
    public LayerMask playerLayer;

    [Header("Attack")]
    public float hitTime = 0.25f;
    public float attackDuration = 0.5f;
    public float attackDelay = 0.4f;   // tốc độ đánh liên tục
    public int damage = 10;

    [Header("Hitbox")]
    public Transform attackPoint;
    public float attackRadius = 0.8f;

    [Header("Health")]
    public int maxHP = 30;

    private int currentHP;
    private State currentState;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;

    private Vector2 startPoint;
    private int moveDir = 1;

    private float attackTimer;
    private float attackLoopTimer;
    private bool hasDealtDamage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        startPoint = transform.position;
        currentHP = maxHP;

        currentState = State.Patrol;
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        DetectPlayer();
        StateMachine();
        FaceDirection();
        UpdateAnimator();
    }

    // =========================
    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRange, playerLayer);
        player = hit ? hit.transform : null;
    }

    // =========================
    void StateMachine()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (player != null) currentState = State.Chase;
                break;

            case State.Chase:
                Chase();

                if (player == null)
                {
                    currentState = State.Patrol;
                    return;
                }

                HandleAttackLoop();
                break;

            case State.Attack:
                Stop();
                HandleAttack();
                break;

            case State.Hurt:
                Stop();
                break;
        }
    }

    // =========================
    void Patrol()
    {
        float left = startPoint.x - patrolDistance;
        float right = startPoint.x + patrolDistance;

        rb.linearVelocity = new Vector2(moveDir * moveSpeed, rb.linearVelocity.y);

        if (transform.position.x > right) moveDir = -1;
        if (transform.position.x < left) moveDir = 1;
    }

    void Chase()
    {
        if (!player) return;

        float dir = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * moveSpeed * chaseSpeedMultiplier, rb.linearVelocity.y);
    }

    void Stop()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    // =========================
    void HandleAttackLoop()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            attackLoopTimer = 0;
            return;
        }

        attackLoopTimer += Time.deltaTime;

        if (attackLoopTimer >= attackDelay)
        {
            StartAttack();
            attackLoopTimer = 0;
        }
    }

    void StartAttack()
    {
        currentState = State.Attack;

        anim.SetTrigger("Attack");

        attackTimer = 0;
        hasDealtDamage = false;
    }

    void HandleAttack()
    {
        attackTimer += Time.deltaTime;

        if (!hasDealtDamage && attackTimer >= hitTime)
        {
            DealDamage();
            hasDealtDamage = true;
        }

        if (attackTimer >= attackDuration)
        {
            currentState = State.Chase;
        }
    }

    // =========================
    //void DealDamage()
    //{
    //    if (!attackPoint)
    //    {
    //        Debug.Log("Missing AttackPoint");
    //        return;
    //    }

    //    Collider2D hit = Physics2D.OverlapCircle(
    //        attackPoint.position,
    //        attackRadius,
    //        playerLayer
    //    );

    //    Debug.Log("Attack check: " + (hit != null));

    //    if (hit)
    //    {
    //        PlayerHealth hp = hit.GetComponent<PlayerHealth>();
    //        if (hp != null)
    //            hp.TakeDamage(damage);
    //    }
    //}
    void DealDamage()
    {
        if (!attackPoint) return;

        Collider2D hit = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRadius,
            playerLayer
        );

        if (hit != null)
        {
            PlayerHealth hp = hit.GetComponentInParent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
    // =========================
    void FaceDirection()
    {
        if (!player) return;

        float dir = player.position.x - transform.position.x;

        if (Mathf.Abs(dir) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(dir) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    // =========================
    void UpdateAnimator()
    {
        if (!anim) return;

        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("IsChasing", currentState == State.Chase);
    }

    // =========================
    public void TakeDamage(int dmg)
    {
        if (currentState == State.Dead) return;

        currentHP -= dmg;

        anim.SetTrigger("Hurt");
        currentState = State.Hurt;

        Invoke(nameof(RecoverFromHurt), 0.25f);

        if (currentHP <= 0)
            Die();
    }

    void RecoverFromHurt()
    {
        if (currentState != State.Dead)
            currentState = State.Chase;
    }

    void Die()
    {
        currentState = State.Dead;

        StopAllCoroutines();
        CancelInvoke();

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        anim.SetTrigger("Die");

        Destroy(gameObject, 2f);
    }

    // =========================
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (attackPoint)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}