using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateManager : MonoBehaviour
{
    public UnityEvent GameStartedEvent;

    private void Awake()
    {
        GameStartedEvent.Invoke();

    }

}
