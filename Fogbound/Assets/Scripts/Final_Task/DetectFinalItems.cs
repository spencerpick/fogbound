using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectFinalItems : MonoBehaviour
{
    public int objectCount = 0;
    public int desiredObjectCount = 3;
    public GameObject outroManager;

    // List to track objects in the trigger
    private List<Collider> objectsInTrigger = new List<Collider>();

    // Update is called once per frame
    void Update()
    {
        // Check if the object count matches the desired count
        if (objectCount == desiredObjectCount)
        {
            outroManager.GetComponent<OutroManager>().playOutro();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Add the object to the list if it's not already in the list
        if (collider.gameObject.layer == LayerMask.NameToLayer("FinalObjects") && !objectsInTrigger.Contains(collider))
        {
            objectsInTrigger.Add(collider);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        // Recalculate the object count by checking all objects currently in the trigger
        objectCount = 0;
        foreach (Collider obj in objectsInTrigger)
        {
            // If the object is still valid (e.g., not deactivated), count it
            if (obj != null && obj.gameObject.activeInHierarchy && !obj.gameObject.GetComponent<TriggerHandler>().isBeingHeld)
            {
                objectCount++;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // Remove the object from the list when it exits
        if (collider.gameObject.layer == LayerMask.NameToLayer("FinalObjects"))
        {
            objectsInTrigger.Remove(collider);
        }
    }
}
