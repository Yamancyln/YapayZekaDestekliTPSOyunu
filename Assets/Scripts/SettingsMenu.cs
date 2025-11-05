using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public Toggle soundToggle;

    private void Start()
    {
        // Oyun başladığında Settings Panel kapalı olsun
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void ToggleSound()
    {
        Debug.Log("Sound: " + soundToggle.isOn);
        // Buraya ileride ses sistemini bağlayabilirsin
        // Örn: AudioListener.volume = soundToggle.isOn ? 1 : 0;
    }
}
