using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    public UnityEvent OnDied;
    public UnityEvent<int> OnUnitHealthChanged;

    #endregion FIELDS

    #region UNITY METHODS

    private void Awake()
    {
        onStart();
    }

    #endregion UNITY METHODS

    #region METHODS

    public void onStart()
    {
        health = maxHealth;
        OnUnitHealthChanged?.Invoke(health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);  // Ensure health stays within bounds
        OnUnitHealthChanged?.Invoke(health);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        OnDied?.Invoke();
    }

    #endregion METHODS
}