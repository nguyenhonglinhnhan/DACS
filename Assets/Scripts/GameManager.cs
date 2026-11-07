using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // =========================================================
    // SINGLETON
    // =========================================================

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // =========================================================
    // UI
    // =========================================================

    [Header("UI")]
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private GameObject gameOverUI; // ← THÊM

    // =========================================================
    // STATE
    // =========================================================

    private bool isGameWin = false;
    private bool isGameOver = false; // ← THÊM

    // =========================================================
    // UNITY
    // =========================================================

    void Start()
    {
        gameWinUI.SetActive(false);
        gameOverUI.SetActive(false); // ← THÊM
    }

    // =========================================================
    // GAME WIN
    // =========================================================

    public void GameWin()
    {
        if (isGameOver) return; // tránh conflict

        isGameWin = true;
        gameWinUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public bool IsGameWin() => isGameWin;

    // =========================================================
    // GAME OVER  ← THÊM TOÀN BỘ PHẦN NÀY
    // =========================================================

    public void GameOver()
    {
        Debug.Log("GameOver() called!"); // ← THÊM DÒNG NÀY

    if (isGameWin)
    {
        Debug.Log("Blocked by isGameWin"); // ← THÊM DÒNG NÀY
        return;
    }

    isGameOver = true;
    gameOverUI.SetActive(true);
    Time.timeScale = 0f;
    
    }

    public bool IsGameOver() => isGameOver;

    // =========================================================
    // BUTTONS
    // =========================================================

    public void GotoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry() // ← THÊM (nút Retry trong GameOver UI)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}