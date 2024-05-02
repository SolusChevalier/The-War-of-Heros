using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class UnitProperties : MonoBehaviour
{
    #region FIELDS

    public UnitTypes unitType;
    public int health;
    public int maxHealth = 100;
    public int attack;
    public int defense;
    public int attackRange;
    public int movementRange;
    public int team;
    public int2 Pos;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        onStart();
    }

    #endregion UNITY METHODS

    #region METHODS

    public void onStart()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);  // Ensure health stays within bounds

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
    }

    #endregion METHODS
}