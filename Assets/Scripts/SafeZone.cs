using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [Header("Win Panel")]
    public GameObject winPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Safe zone reached!");
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("WinPanel atanmamış!");
            }

            Time.timeScale = 0f; // oyunu durdur
        }
    }
}


