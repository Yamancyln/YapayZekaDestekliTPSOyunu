using UnityEngine;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    private int currentHealth = 100;
    public Animator animator;
    public Slider healthBar; // 👈 Bu satır Health Bar alanını Inspector’a ekler!

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
            Destroy(gameObject, 5f); // 5 saniye sonra yok zombiyi yok eder
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }
}
