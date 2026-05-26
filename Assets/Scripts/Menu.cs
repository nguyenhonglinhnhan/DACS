using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LodGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Dừng play mode trong Editor
#else
        Application.Quit(); // Thoát app khi đã build
#endif
    }
}