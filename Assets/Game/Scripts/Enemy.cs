using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : AttackableObject
{

    [NonSerialized]
    public float attackRange = 0.8f;
    public bool isIdleAtStart = false;

    public bool IsAlive
    {
        get
        {
            return currentHealth > 0;
        }
    }

    private Animator animator;
    private NavMeshAgent agent;
    private Controls controls;
    
    private static readonly int animFlagIsWalking = Animator.StringToHash("IsWalking");

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        controls = Controls.Instance;
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
        checkPlayerDistance();
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
    
    private void checkPlayerDistance()
    {
        if (!IsAlive)
            return;
        
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < attackRange)
        {
            controls.GameOver();
        }
    }
    
    public override void Die()
    {
        base.Die();
        agent.enabled = false;
    }

    public void AttackPlayer()
    {
        if (IsAlive)
        {
            agent.enabled = true;
            agent.destination = player.transform.position;
        }
    }
}
