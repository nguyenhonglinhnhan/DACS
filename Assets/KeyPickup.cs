using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Có gì đó chạm Key: " + other.name);

        if (!other.CompareTag("Player")) return;
        
        Debug.Log("Player chạm Key!");
        
        if (other.GetComponent<PlayerController>() == null)
        {
            Debug.Log("Không tìm thấy PlayerController!");
            return;
        }

        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        
        Debug.Log("GameManager: " + gm);
        
        if (gm != null)
            gm.GameWin();

        Destroy(gameObject);
    }
}