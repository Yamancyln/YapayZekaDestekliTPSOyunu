using System.Collections.Generic;
using UnityEngine;

// Bu script chatGPT den yardım alınarak yapıldı
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private List<Zombie> activeZombies = new List<Zombie>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterZombie(Zombie z)
    {
        if (!activeZombies.Contains(z))
            activeZombies.Add(z);
    }

    public void UnregisterZombie(Zombie z)
    {
        if (activeZombies.Contains(z))
            activeZombies.Remove(z);

        if (activeZombies.Count == 0)
        {
            // 🟥 Tüm zombiler öldü ⇒ oyun bitti
            FindObjectOfType<PauseMenu>().GameOverState();
        }
    }
}