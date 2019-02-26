using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    [Range(1f, 200f)]
    public float TotalHealth = 100;
    [Range(1f, 200f)]
    public float CurrentHealth;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = TotalHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damageTotal, GameObject damageSource)
    {
        CurrentHealth = damageTotal > CurrentHealth ? 0 : CurrentHealth - damageTotal;
        Debug.Log("I, the mighty " + name + " took damage from " + damageSource.name + "! + Now I only have " + CurrentHealth + " hitpoints left!" );
    }

    public void Heal(float healing)
    {
        CurrentHealth = CurrentHealth + healing > TotalHealth ? TotalHealth : CurrentHealth + healing;
    }

    public void Destroy()
    {

    }

    // This needs to broadcast messages about the actions that occur in order to have an AI/Aggro system that reacts the player's actions.
}

