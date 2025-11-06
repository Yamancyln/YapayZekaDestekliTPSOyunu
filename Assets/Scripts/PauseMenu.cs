using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;       // Panel veya Canvas referansı
    public Button resumeButton;
    public static bool GameIsPaused = false; // Global flag (ThirdPersonController scriptinden erişilir)
    private bool isGameOver = false;

    void Awake()
    {
        Time.timeScale = 1f;             // Oyun başında zaman normal olsun
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;            
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;            
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //public void GoToMainMenu()
    //{
    //Time.timeScale = 1f; // oyunu normale döndür
    //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    //}

    public void QuitGame()
    {
        Debug.Log("Oyun kapatılıyor...");
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // oyunu play modundan çıkararak oyunu kapatmış olur. Not: Kod chatGPT'den alındı.
    #endif
    }

    // chatGPT yardımı alındı
    public void GameOverState()
    {
        isGameOver = true;
        Time.timeScale = 2f;
        pauseMenuUI.SetActive(true);

        if (resumeButton != null) 
        {
            resumeButton.interactable = false;
            resumeButton.gameObject.SetActive(false);
        }            

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;        
    }
}
