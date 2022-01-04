using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    // Variables
    private float currentHealth;
    private float maxHealth;
    private Image healthImage;

    private Slider healthSlider; 
  
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = GetComponent<TankData>().tankHP;
        currentHealth = maxHealth;
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.maxValue = maxHealth;
        healthImage = healthSlider.fillRect.GetComponent<Image>();
        UpdateSlider();
    }

    public void TakeDamage(TankData hitBy)
    {
        currentHealth -= hitBy.bulletDamage;
        UpdateSlider();
        if (currentHealth <= 0) 
        {
            hitBy.score += GetComponent<TankData>().tankPointValue;
            Die(); 
        }
    }

    public void HealDamage(float damage)
    {
        currentHealth = Mathf.Min(currentHealth + damage, maxHealth);
        UpdateSlider();
    }

    public void IncreaseMaxHelth(float maxHealthIncrease)
    {
        maxHealth += maxHealthIncrease;
        currentHealth += maxHealthIncrease;
        UpdateSlider();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void UpdateSlider()
    {
        healthSlider.value = currentHealth;
        Debug.Log(currentHealth + " : " + maxHealth * (2f / 3f));
        if(currentHealth > maxHealth * (2f / 3f))
        {
            healthImage.color = Color.green;
        }
        else if (currentHealth > maxHealth * (1f / 3f))
        {
            healthImage.color = Color.yellow;
        }
        else
        {
            healthImage.color = Color.red;
        }
    }

    public float GetCurrentHP()
    {
        return currentHealth;
    }
}
