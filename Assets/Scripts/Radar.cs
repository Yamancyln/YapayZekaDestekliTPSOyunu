using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Radar : MonoBehaviour
{
    [Header("Radar Settings")]
    public Transform player; // Oyuncu
    public RectTransform radarUI; // RadarBackground objesi
    public RectTransform radarIconsParent; // Noktaların oluşturulacağı yer
    public GameObject blipPrefab; // Kırmızı nokta prefab'ı
    public float radarRange = 50f; // Radarın menzili
    public float radarSize = 150f; // Radarın boyutu (UI ölçeği)

    private List<GameObject> blipObjects = new List<GameObject>();

    void Update()
    {
        // Eski blipleri temizle
        foreach (GameObject blip in blipObjects)
            Destroy(blip);
        blipObjects.Clear();

        // Tüm zombileri bul
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Zombie");

        foreach (GameObject enemy in enemies)
        {
            Vector3 offset = enemy.transform.position - player.position;
            float distance = offset.magnitude;

            if (distance <= radarRange)
            {
                // Noktanın açı ve uzaklık hesabı
                float angle = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg - player.eulerAngles.y;
                float normalizedDistance = distance / radarRange;
                float blipX = normalizedDistance * Mathf.Sin(angle * Mathf.Deg2Rad) * radarSize;
                float blipY = normalizedDistance * Mathf.Cos(angle * Mathf.Deg2Rad) * radarSize;

                // Nokta oluştur
                GameObject blip = Instantiate(blipPrefab, radarIconsParent);
                blip.GetComponent<RectTransform>().anchoredPosition = new Vector2(blipX, blipY);
                blipObjects.Add(blip);
            }
        }
    }
}
