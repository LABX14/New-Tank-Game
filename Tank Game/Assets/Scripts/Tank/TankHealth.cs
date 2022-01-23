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
    private TankData data;
  
    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<TankData>();
        maxHealth = data.tankHP;
        currentHealth = maxHealth;
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.maxValue = maxHealth;
        healthImage = healthSlider.fillRect.GetComponent<Image>();
        UpdateSlider();
    }

    // Upon getting by the bullet, the bullet will take damage which will lower its health
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

    // The player's health value will restore the amount of health the healing value was set too
    public void HealDamage(float damage)
    {
        currentHealth = Mathf.Min(currentHealth + damage, maxHealth);
        UpdateSlider();
    }

    // This will set the max health of the player to a set higher value
    public void IncreaseMaxHelth(float maxHealthIncrease)
    {
        maxHealth += maxHealthIncrease;
        currentHealth += maxHealthIncrease;
        healthSlider.maxValue = maxHealth;
        UpdateSlider();
    }


    // When the player dies,
    private void Die()
    {
        //play the audio clip at the position where that tank died
        AudioSource.PlayClipAtPoint(data.tankDeathSound, transform.position, GameManager.instance.sfxVolume);
        
        // Update the score and drop the number of lives 
        if (GetComponent<InputController>())
        {
            int playerIndex = GetComponent<InputController>().playerIndex;
            GameManager.instance.players[playerIndex].UpdateScore();
            GameManager.instance.players[playerIndex].lives--;
        }

        Destroy(gameObject);
    }

    // Update the slider if the player takes damage
    private void UpdateSlider()
    {
        healthSlider.value = currentHealth;
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
    
    // Getting the player's current health
    public float GetCurrentHP()
    {
        return currentHealth;
    }
}
