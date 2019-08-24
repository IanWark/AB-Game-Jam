using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 1000;
    
    public float speed = 5;

    public bool controlEnabled = true;
    
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Set health to maximum
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            // This may be useful if we want to make the movement better: https://roystan.net/articles/character-controller-2d.html

            // Simple x-axis only movement
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0));
        }
    }
    
    public void OnHit(int damage)
    {
        currentHealth -= damage;

        healthSlider.value = currentHealth;
    }
}
