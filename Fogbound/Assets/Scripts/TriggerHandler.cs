using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public bool isBeingHeld = false;
    public delegate void TriggerEvent(Collider other);
    public event TriggerEvent OnEnter;
    public event TriggerEvent OnExit;

    private void OnTriggerEnter(Collider other)
    {
        OnEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnExit?.Invoke(other);
    }
}