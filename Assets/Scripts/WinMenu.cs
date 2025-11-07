using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public void RestartLevel()
    {
        Time.timeScale = 1f; // oyunu devam ettir
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // sahneyi yeniden yükle
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Time.timeScale = 1f;

        // Eğer ana menü sahnen varsa, onun adını buraya yaz:
        // SceneManager.LoadScene("MainMenu");

        // Eğer tamamen çıkmak istiyorsan:
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // editörde testi durdurur
#endif
    }
}
