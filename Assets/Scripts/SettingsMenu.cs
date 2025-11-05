using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Toggle soundToggle;
    public Toggle musicToggle;

    private void Start()
    {

        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ToggleSound()
    {
        Debug.Log("Sound: " + soundToggle.isOn);
        // Buraya ses sistemin varsa bağlayabilirsin.
    }

    public void ToggleMusic()
    {
        Debug.Log("Music: " + musicToggle.isOn);
        // Buraya müzik sistemi bağlanacak. ddd
    }
}
