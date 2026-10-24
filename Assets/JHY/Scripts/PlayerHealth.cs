using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public int currentHealth;
    public int maxHealth = 20;
    public TMP_Text health;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentHealth = maxHealth;
        health.text = "Health : " + currentHealth;
    }
    private void Update()
    {
        health.text = "Health : " + currentHealth;
    }
    public void RestoreHealth(int heal)
    {
        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
        health.text = "Health : " + currentHealth;
    }
}
