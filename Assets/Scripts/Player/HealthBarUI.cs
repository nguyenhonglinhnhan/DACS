using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [Header("Cấu hình UI")]
    public Image healthFillImage;      // Kéo cái ảnh thanh màu đỏ (Fill) vào đây

    [Header("Theo dõi Nhân vật")]
    public Transform playerTransform;  // Kéo Object player vào đây
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Khoảng cách trên đầu nhân vật (Chỉnh ở đây)

    private float maxHealth;

    // Hàm cài đặt máu tối đa ban đầu
    public void InitializeHealth(float maxHP)
    {
        maxHealth = maxHP;
        UpdateHealthBar(maxHP);
    }

    // Hàm cập nhật thanh máu co giãn theo phần trăm
    public void UpdateHealthBar(float currentHP)
    {
        if (healthFillImage != null && maxHealth > 0)
        {
            // Tính tỷ lệ phần trăm máu từ 0 -> 1
            healthFillImage.fillAmount = currentHP / maxHealth;
        }
    }

    void LateUpdate()
    {
        // Luôn giữ thanh máu bám theo vị trí của Player cộng thêm một khoảng cao hơn (offset)
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
        }
    }
}