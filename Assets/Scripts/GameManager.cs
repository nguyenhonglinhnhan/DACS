using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameWinUI;
    private bool isGameWin = false;
    void Start()
    {
        GameWinUI.SetActive(false);
    }
    
    public void GameWin()
    {
        isGameWin = true;
        GameWinUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public bool IsGameWin()
    {
        return isGameWin;
    }
    public void GotoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
}
