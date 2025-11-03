using UnityEngine;
using UnityEngine.UI; // Burası önemli!

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isDead = false;
    public Slider healthBar; // 👈 Bu satır Health Bar alanını Inspector’a ekler!

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.maxValue = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " öldü!");

        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("die");

        if (gameObject.CompareTag("Enemy"))
            Destroy(gameObject, 3f);
    }
}
