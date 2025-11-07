using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public GameObject winPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Safe zone reached!");
            winPanel.SetActive(true);

            // Oyunu durdur
            Time.timeScale = 0f;

            // Farenin görünür ve serbest olmasını sağla
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // (Opsiyonel) Oyuncunun hareket scriptini devre dışı bırak
            // other.GetComponent<PlayerMovement>().enabled = false;
        }
    }
}
