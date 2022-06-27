using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public event Action OnThrowAnimation;

    public void OnThrow()
    {
        OnThrowAnimation?.Invoke();
    }
}
