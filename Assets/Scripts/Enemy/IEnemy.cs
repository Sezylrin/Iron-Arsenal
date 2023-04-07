using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    EnemyManager Manager { get; set; }
    GameObject Player { get; set; }
    Rigidbody EnemyRB { get; set; }
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    float DamageOnCollide { get; set; }
    float Speed { get; set; }
    void TakeDamage(float damage);
    void OnDeath();
}