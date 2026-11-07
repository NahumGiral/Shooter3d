using UnityEngine;

/// <summary>
/// Componente para cualquier objeto o enemigo que pueda recibir daño y ser destruido.
/// </summary>
public class Target : MonoBehaviour
{
    [Header("Estadísticas")]
    [Tooltip("Puntos de vida máximos")]
    public float maxHealth = 50f;

    private float currentHealth;

    [Header("Efectos (Opcional)")]
    [Tooltip("Efecto de partículas al destruirse")]
    public GameObject destructionEffect;

    void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Llamado cuando un arma o proyectil impacta este objetivo.
    /// </summary>
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} de daño. Vida restante: {currentHealth}");

        // Efecto rápido de "hit feedback": hacer parpadear el color brevemente
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.red;
            Invoke(nameof(ResetColor), 0.1f);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void ResetColor()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.white;
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} fue destruido!");

        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, transform.rotation);
        }

        // Destruir el objeto en la escena
        Destroy(gameObject);
    }
}
