using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateManager : MonoBehaviour
{
    [Header("Public Unity Events")]

    public UnityEvent GameStartedEvent;

    public UnityEvent GameLoadedEvent;

    public UnityEvent PlayStartedEvent;

    public UnityEvent SpawningStartedEvent;

    private void Awake()
    {
        Debug.Log("Target frame rate = " + Application.targetFrameRate);

        // Invoke game started event. 
        // This event instantiats the starting objects & settings. 
        GameStartedEvent.Invoke();

    }

    public void StartGameLoadedState()
    {
        GameLoadedEvent.Invoke();

    }

    public void StartSpawningStartedState()
    {
        SpawningStartedEvent.Invoke();

    }

}
