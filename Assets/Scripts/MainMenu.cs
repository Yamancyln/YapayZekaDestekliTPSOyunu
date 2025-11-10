using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // ✅ Menüde yazı göstermek için eklendi

public class MainMenu : MonoBehaviour
{
    public string gameSceneName;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI infoText; // chatgptden yardım alındı 

    private string missionMessage = "Tek bir amacın var: Yeşil Bölge'yi BUL ve Çağrıyı Yap! Hayatta kal, ajan..";

    private void Start()
    {
        // ✅ Oyun başında menüde görülecek yazı
        if (infoText != null)
            infoText.text = missionMessage;
        else
            Debug.LogWarning("InfoText (TextMeshProUGUI) atanmamış!");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed!");
    }

    // ✅ İstersen başka bir butondan çağırabileceğin metot:
    public void SetMenuText(string newText)
    {
        if (infoText != null)
            infoText.text = newText;
    }
}
