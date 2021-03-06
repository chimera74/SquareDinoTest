using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;
    public int damage = 1;
    public float pushbackForce = 10f;
    public float pushbackRadius = 10f;

    private Rigidbody rb;
    private bool hasCollided = false;
    private ObjectPool pool;
    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pool = Player.Instance.GetComponent<ObjectPool>();
    }

    public void Launch(Vector3 direction)
    {
        rb.velocity = direction.normalized * speed;
        hasCollided = false;
        this.Invoke(() => { pool.Release(gameObject); }, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided && collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<AttackableObject>();
            enemy.TakeDamage(damage);
            enemy.ApplyForceAt(collision.GetContact(0).point, pushbackForce, pushbackRadius);
        }
        hasCollided = true;
    }
}
