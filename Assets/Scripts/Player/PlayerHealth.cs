using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("=== CÀI ĐẶT MÁU ===")]
    public float maxHP = 100f;
    [SerializeField] private float currentHP;

    [Header("=== TỰ ĐỘNG KẾT NỐI UI ===")]
    private Image healthBarFill;
    private Text healthText;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isDead;
    private Coroutine fillCoroutine;
    private float animationSpeed = 0.2f; // Tốc độ rút máu mượt

    void Start()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // THẦN CHÚ: Tự động đi lùng sục trong các Object con để tìm Thanh máu và Chữ số
        healthBarFill = GetComponentInChildren<Image>(true);
        healthText = GetComponentInChildren<Text>(true);

        // Khởi tạo hiển thị lúc đầu game
        UpdateHealthUI(currentHP, currentHP);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        float previousHP = currentHP;
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        Debug.Log($"Player dính {damage} sát thương. Máu còn: {currentHP}/{maxHP}");

        // Cập nhật thanh máu ngay lập tức không qua trung gian
        UpdateHealthUI(previousHP, currentHP);

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    // Hàm hỗ trợ phòng hờ script của quái gọi sai tên hàm
    public void TakeDamageFromEnemy(float damage)
    {
        TakeDamage(damage);
    }

    // Logic tự xử lý co giãn thanh máu và hiển thị chữ
    private void UpdateHealthUI(float oldHP, float newHP)
    {
        // 1. Cập nhật chữ hiển thị chuẩn 100%
        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(newHP)}/{Mathf.RoundToInt(maxHP)}";
        }

        // 2. Co rút thanh máu mượt mà
        if (healthBarFill != null)
        {
            if (gameObject.activeInHierarchy)
            {
                if (fillCoroutine != null) StopCoroutine(fillCoroutine);
                fillCoroutine = StartCoroutine(AnimateFill(oldHP / maxHP, newHP / maxHP));
            }
            else
            {
                healthBarFill.fillAmount = newHP / maxHP;
            }
        }
    }

    private IEnumerator AnimateFill(float startFill, float targetFill)
    {
        float timeElapsed = 0;
        while (timeElapsed < animationSpeed)
        {
            healthBarFill.fillAmount = Mathf.Lerp(startFill, targetFill, timeElapsed / animationSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        healthBarFill.fillAmount = targetFill;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Died");
        if (anim != null) anim.SetTrigger("Die");
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        this.enabled = false;
    }
}