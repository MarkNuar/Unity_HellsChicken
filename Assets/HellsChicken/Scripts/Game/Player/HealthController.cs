using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int numberOfHearts;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    
    // Update is called once per frame
    private void Update()
    {
        if (health > numberOfHearts)
            health = numberOfHearts;
        for (var i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < health ? fullHeart : emptyHeart;
            hearts[i].enabled = i < numberOfHearts;
        }
    }

    public void IncreaseHealth()
    {
        if(health < numberOfHearts)
            health++;
    }

    public void DecreaseHealth()
    {
        if (health > 0)
            health--;
    }
}
