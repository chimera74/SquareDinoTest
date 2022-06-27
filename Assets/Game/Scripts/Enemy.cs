using UnityEngine;
using UnityEngine.AI;

public class Enemy : AttackableObject
{
    
    public bool isIdleAtStart = false;

    private Animator animator;
    private NavMeshAgent agent;
    
    private static readonly int animFlagIsWalking = Animator.StringToHash("IsWalking");

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public override void SetRagdollState(bool state)
    {
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
            rb.isKinematic = !state;
        foreach (var col in GetComponentsInChildren<Collider>())
            col.isTrigger = !state;
        animator.enabled = !state;
        mainCollider.isTrigger = state;
    }

    protected void Update()
    {
        updateAnimations();
    }

    private void updateAnimations()
    {
        float velocityNormalized = agent.velocity.magnitude / agent.speed;
        if (velocityNormalized < 0.01f)
        {
            animator.SetBool(animFlagIsWalking, false);
        }
        else
        {
            animator.SetBool(animFlagIsWalking, true);
        }
    }
    
    public override void Die()
    {
        base.Die();
        agent.enabled = false;
    }

    public void AttackPlayer()
    {
        agent.enabled = true;
        agent.destination = player.transform.position;
    }
}
