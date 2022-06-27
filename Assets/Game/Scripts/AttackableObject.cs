using UnityEngine;
using UnityEngine.Events;

public class AttackableObject : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;
    
    public UnityEvent onDeathEvent;
    
    protected Player player;
    protected Collider mainCollider;
    
    protected virtual void Awake()
    {
        player = Player.Instance;
        mainCollider = GetComponent<Collider>();
    }
    
    protected void Start()
    {
        currentHealth = maxHealth;
        SetRagdollState(false);
    }
    
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public virtual void Die()
    {
        SetRagdollState(true);
        Destroy(gameObject, 5f);
        onDeathEvent?.Invoke();
    }
    
    public virtual void SetRagdollState(bool state)
    {
        GetComponent<Rigidbody>().isKinematic = !state;
    }
    
    public void ApplyForceAt(Vector3 position, float force, float radius)
    {
        if (force <= 0)
            return;
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(force, position, radius);
            }
        }
    }
}
