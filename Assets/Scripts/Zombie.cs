using UnityEngine;
using UnityEngine.UI;

// GDTitans youtube kanalının "Unity Third Person Game" video serisindeki "3D ENEMY AI in UNITY - (E03): TAKE DAMAGE & DEATH" ve "3D ENEMY AI in UNITY - (E04): HEALTH BAR" adlı videodan yardım alınmıştır.

public class Zombie : MonoBehaviour
{
    private int currentHealth = 100;
    public Animator animator;
    public Slider healthBar;    // Bu satır Health Bar alanını Inspector’a ekler!

    private void Start()
    {
        if (healthBar != null)
            healthBar.maxValue = currentHealth;

        GameManager.Instance.RegisterZombie(this); // zombiyi kayıt et //chatGPT yardımı alındı
    }

    void Update()
    {
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            animator.SetTrigger("die");
            GameManager.Instance.UnregisterZombie(this); // zombiyi sil  //chatGPT yardımı alındı
            Destroy(gameObject, 5f);          // 5 saniye sonra yok zombiyi yok eder
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }
}