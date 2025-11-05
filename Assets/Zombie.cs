using UnityEngine;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    public int maxHealth = 100;
    public Animator animator;
    public Slider healthBar; // 👈 Bu satır Health Bar alanını Inspector’a ekler!
    public int currentHealth;

    public void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.maxValue = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0) 
        {
            animator.SetTrigger("die");
            Destroy(gameObject, 3f); // 3 saniye sonra yok zombiyi yok eder
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }
}
