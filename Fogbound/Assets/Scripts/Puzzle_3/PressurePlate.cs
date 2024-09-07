using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isPressed;
    public GameObject plate;

    public float desiredPressure;
    private float currentPressure;
    
    private Vector3 offset = new Vector3(0, 0.06f, 0);
    private Vector3 startingPos;

    private TextMeshPro desiredPressureText;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = plate.transform.localPosition;

        desiredPressureText = GetComponentInChildren<TextMeshPro>();
        desiredPressureText.text = desiredPressure.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Animate if there is pressure
        plate.transform.localPosition = (currentPressure > 0) ? offset : startingPos;
        isPressed = currentPressure > 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        // A weighted object has been removed
        if (collider.gameObject.layer == LayerMask.NameToLayer("WeightedObjects"))
        {
            AddPressure(collider.GetComponent<WeightedObject>().weight);
        }
        
    }

    private void OnTriggerExit(Collider collider)
    {
        // A weighted object has been added
        if (collider.gameObject.layer == LayerMask.NameToLayer("WeightedObjects"))
        {
            RemovePressure(collider.GetComponent<WeightedObject>().weight);
        }
    }

    void AddPressure(float weight) => currentPressure += weight;

    void RemovePressure(float weight) => currentPressure -= weight;

}
