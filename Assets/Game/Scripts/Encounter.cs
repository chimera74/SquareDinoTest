using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Encounter : MonoBehaviour
{

    public Transform wp;
    public Transform cameraTarget;
    public Enemy[] enemies;
    public Encounter nextEncounter;
    public bool firstEncounter = false;
    private Player player;
    private Controls controls;

    protected void Awake()
    {
        player = Player.Instance;
        controls = Controls.Instance;
    }

    protected void Start()
    {
        if (firstEncounter)
        {
            InitializeEncounter();
            StartEncounter();
        }
    }

    protected void OnDisable()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
                enemy.onDeathEvent.RemoveListener(CheckCompletion);
        }
    }

    public void CheckCompletion()
    {
        bool allDead = true;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.currentHealth > 0)
            {
                allDead = false;
                break;
            }
        }
        if (allDead)
        {
            if (nextEncounter != null)
            {
                cleanUp();
                MoveToNextEncounter();
            }
            else
            {
                cleanUp();
                controls.Victory();
            }
        }
    }

    public void InitializeEncounter()
    {
        player.OnDestinationReached += StartEncounter;
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
                enemy.onDeathEvent.AddListener(CheckCompletion);
        }
    }

    private void cleanUp()
    {
        player.OnDestinationReached -= StartEncounter;
    }

    public void StartEncounter()
    {
        player.LookAt(cameraTarget.position);
        CheckCompletion();
            
        foreach (Enemy enemy in enemies)
        {
            if (!enemy.isIdleAtStart)
                enemy.AttackPlayer();
        }
    }

    public void MoveToNextEncounter()
    {
        nextEncounter.InitializeEncounter();
        player.MoveToWaypoint(nextEncounter.wp.position);
    }
}
