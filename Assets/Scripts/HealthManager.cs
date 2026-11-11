using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public bool isBase = false;
    public GameObject gameOverPanel;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " получил урон. Здоровье: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isBase)
        {
            Debug.Log("База уничтожена!");
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
            Time.timeScale = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
