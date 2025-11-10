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

    // GÖREV METNİ ENTEGRASYONU:
    // Bu metin, Inspector'da büyük bir kutu içinde görünür ve Pause Menu'nüzdeki UI Text bileşenine elle yapıştırılmalıdır.
    [Header("GÖREV METNİ (Kopyalamak İçin)")]
    [TextArea(10, 20)] // 10 satır minimum, 20 satır maksimum yükseklik
    public string missionTextToCopy = 
        "GÖREV:\n\n" +
        "Dinle Kenshin, burası son temas. Etrafındaki her şey düştü. Artık ekibin yok. Sadece sen kaldın. Elindeki tüfek, kalabalık sürülere karşı son savunman.\n\n" +
        "Unutma: Görevimiz Basit. Telsizden gelen cızırtılı son koordinatlara odaklan. O lanet olası 'Yeşil Bölge'nin tam yerini bilmiyoruz, bu yüzden bulman gerek. O bölge, çalışan bir radyo istasyonu demek. Başka hiçbir yerde iletişim kuramıyoruz.\n\n" +
        "İnsanlık için yardım çağrısı yapabileceğimiz son umut noktası o gizli bölge. Yıkılmış binaların arasından, bitmek bilmeyen zombi uğultusuna karşı çelik gibi iradenle ilerleyeceksin. Geride kalan herkesin gözü sende.\n\n" +
        "Tek bir amacın var: Yeşil Bölge'yi BUL ve Çağrıyı Yap! Hayatta kal, ajan.";


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
        // oyunu play modundan çıkararak oyunu kapatmış olur. Not: Kod chatGPT'den alındı.
        UnityEditor.EditorApplication.isPlaying = false;  
    #endif
    }

    // chatGPT yardımı alındı
    public void GameOverState()
    {
        isGameOver = true;
        Time.timeScale = 4f;
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