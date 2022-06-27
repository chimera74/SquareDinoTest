using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Controls : MonoBehaviour
{
    private Player player;

    protected void Awake()
    {
        player = Player.Instance;
    }

    protected void Update()
    {
        processTouches();        
    }

    private void processTouches()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                Vector3 target;
                if (Physics.Raycast(ray, out RaycastHit hit, 30f))
                    target = hit.point;
                else
                    target = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.farClipPlane));
                player.Throw(target);
            }
        }
    }
}
