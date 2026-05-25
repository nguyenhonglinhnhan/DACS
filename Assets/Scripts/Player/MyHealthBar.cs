using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FollowCameraRotation))]
public class MyHealthBar : MonoBehaviour
{
    [SerializeField] bool isBillboarded = true;
    [SerializeField] bool shouldShowHealthNumbers = true;
    [SerializeField] float animationSpeed = 0.2f; // Tốc độ rút máu mượt

    [Header("Kéo thả UI con vào đây")]
    [SerializeField] Image image; // Kéo object "Health Bar Fill" vào đây
    [SerializeField] Text text;   // Kéo object "Text" vào đây

    private FollowCameraRotation followCameraRotation;
    private Coroutine fillCoroutine;

    private void Start()
    {
        followCameraRotation = GetComponent<FollowCameraRotation>();

        // Tự động tìm linh kiện nếu quên kéo thả
        if (image == null) image = GetComponentInChildren<Image>();
        if (text == null) text = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (followCameraRotation != null)
            followCameraRotation.enabled = isBillboarded;

        if (text != null)
            text.enabled = shouldShowHealthNumbers;
    }

    // Hàm nhận lệnh từ Player để đổi số và co thanh máu
    public void ChangeHealthFill(CurrentHealth currentHealth)
    {
        // 1. Cập nhật số chữ hiển thị (Lấy max mặc định hoặc dựa trên tỷ lệ)
        if (text != null)
        {
            // Ép số hiển thị tối đa là 100 theo cấu trúc máu mới của ní
            float maxCalculated = currentHealth.current / (currentHealth.percentage / 100f);
            if (float.IsNaN(maxCalculated) || float.IsInfinity(maxCalculated)) maxCalculated = 100f;

            text.text = $"{Mathf.RoundToInt(currentHealth.current)}/{Mathf.RoundToInt(maxCalculated)}";
        }

        // 2. Chạy hiệu ứng co thanh máu mượt mà từ ảnh fill của ní
        if (image != null && gameObject.activeInHierarchy)
        {
            if (fillCoroutine != null) StopCoroutine(fillCoroutine);
            fillCoroutine = StartCoroutine(ChangeFillAmount(currentHealth.percentage / 100f));
        }
    }

    private IEnumerator ChangeFillAmount(float targetFill)
    {
        float startFill = image.fillAmount;
        float timeElapsed = 0;

        while (timeElapsed < animationSpeed)
        {
            image.fillAmount = Mathf.Lerp(startFill, targetFill, timeElapsed / animationSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        image.fillAmount = targetFill;
    }
}