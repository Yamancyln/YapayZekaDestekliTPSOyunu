using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    public Animator animator;
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            animator.SetTrigger("Die");
            
            FindObjectOfType<PauseMenu>().GameOverState(); // PauseMenu scriptindeki sınıfı içerisinde GameOverState() fonksiyonunu bulur ve aktif eder.
            Destroy(gameObject, 5f); // 5 saniye sonra oyuncuyu sahneden sil
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
}
