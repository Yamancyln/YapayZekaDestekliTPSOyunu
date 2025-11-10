using UnityEngine;
using System.Collections.Generic;

public class Radar : MonoBehaviour
{
    [Header("Radar Settings")]
    public Transform player;                     // Oyuncu referansı
    public RectTransform radarUI;                // Radarın kendisi (örneğin RadarBackground)
    public Transform radarIconsParent;           // Radar bliplerinin tutulduğu alan
    public GameObject enemyBlipPrefab;           // Kırmızı nokta prefab'ı
    public float radarRange = 25f;               // Sadece bu menzil içindekiler görünsün
    public float radarScale = 2f;                // Radar üzerindeki ölçek oranı (deneme ile ayarlanabilir)

    private readonly List<GameObject> enemyBlips = new();

    void Update()
    {
        // Eski blipleri temizle
        foreach (var blip in enemyBlips)
            Destroy(blip);
        enemyBlips.Clear();

        // Sahnedeki tüm zombileri bul
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Zombie");

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(player.position, enemy.transform.position);

            // Radar menzili dışındaki zombileri gösterme
            if (dist > radarRange)
                continue;

            // Blip oluştur
            GameObject blip = Instantiate(enemyBlipPrefab, radarIconsParent);
            enemyBlips.Add(blip);

            // Blip boyutunu büyüt (örnek: 30x30)
            RectTransform blipRect = blip.GetComponent<RectTransform>();
            if (blipRect != null)
                blipRect.sizeDelta = new Vector2(30f, 30f);

            // Oyuncunun yönünü hesaba katarak pozisyon hesapla
            Vector3 dir = enemy.transform.position - player.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg - player.eulerAngles.y;

            // Radar üzerindeki konumu bul
            float normalizedDist = dist / radarRange;
            float radius = (radarUI.rect.width / 2f) * normalizedDist;

            float x = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = radius * Mathf.Cos(angle * Mathf.Deg2Rad);

            blipRect.anchoredPosition = new Vector2(x, y);
        }
    }
}
