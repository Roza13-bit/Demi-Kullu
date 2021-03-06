﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateManager : MonoBehaviour
{
    [Header("Public Unity Events")]

    public UnityEvent GameStartedEvent;

    public UnityEvent GameLoadedEvent;

    public UnityEvent PlayStartedEvent;

    private void Awake()
    {
        // Invoke game started event. 
        // This event instantiats the starting objects & settings. 
        GameStartedEvent.Invoke();

    }

    public void StartPlayStartedState()
    {
        GameLoadedEvent.Invoke();

    }

}
