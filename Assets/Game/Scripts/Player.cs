using System;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event Action OnDestinationReached;
    
    [Header("Settings")]
    public Transform throwPoint;
    
    private Vector3 target;
    private bool isMoving; 
    
    private NavMeshAgent agent;
    private Animator animator;
    private Transform childTransform;
    private AnimationEvents animationEvents;
    private ObjectPool objectPool;
    
    private static readonly int animFlagIsRunning = Animator.StringToHash("IsRunning");
    private static readonly int animTriggerThrow = Animator.StringToHash("Throw");

    protected void Awake()
    {
        Instance = this;
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<AnimationEvents>();
        childTransform = transform.GetChild(0);
        objectPool = GetComponent<ObjectPool>();
    }

    protected void Start()
    {
        animationEvents.OnThrowAnimation += OnThrow;
        agent.enabled = false;
    }

    protected void OnDisable()
    {
        animationEvents.OnThrowAnimation -= OnThrow;
    }

    protected void Update()
    {
        updateAnimations();
        isDestinationReached();
    }

    private void updateAnimations()
    {
        float velocityNormalized = agent.velocity.magnitude / agent.speed;
        if (velocityNormalized < 0.01f)
        {
            animator.SetBool(animFlagIsRunning, false);
        }
        else
        {
            animator.SetBool(animFlagIsRunning, true);
        }
    }

    private void isDestinationReached()
    {
        if (!isMoving)
            return;
            
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    OnDestinationReached?.Invoke();
                    isMoving = false;
                    agent.enabled = false;
                }
            }
        }
    }

    public void MoveToWaypoint(Vector3 waypoint)
    {
        faceForward();
        agent.enabled = true;
        agent.destination = waypoint;
        isMoving = true;
    }
    
    public void LookAt(Vector3 lookAtTarget) {
        transform.LookAt(new Vector3(lookAtTarget.x, transform.position.y, lookAtTarget.z));
    }

    public void Throw(Vector3 direction)
    {
        if (isMoving)
            return;
        
        target = direction;
        faceTarget(direction);
        animator.SetTrigger(animTriggerThrow);
    }
    
    public void OnThrow()
    {
        SpawnProjectile();
    }

    private void faceTarget(Vector3 direction)
    {
        var t = new Vector3(direction.x, childTransform.transform.position.y, direction.z);
        childTransform.LookAt(t);
        Debug.DrawLine(transform.position, t);
    }
    
    private void faceForward()
    { 
        childTransform.LookAt(transform.position + transform.forward);
    }
    
    private void SpawnProjectile()
    {        
        var spawnPos = throwPoint.position;
        var projectile = objectPool.Get();
        if (projectile != null)
        {
            projectile.transform.position = spawnPos;
            projectile.GetComponent<Projectile>().Launch(target - spawnPos);
        }
    }
}
