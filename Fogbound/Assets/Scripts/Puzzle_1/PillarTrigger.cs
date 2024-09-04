using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarTrigger : MonoBehaviour
{
    public int pillarNumber; // The number of the pillar this is attached to
    private bool isToyOnPillar = false; // Track whether toy is on pillar or not

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Toy") && isToyOnPillar == false)
        {
            Debug.Log("IM IN THE TRIGGGERRRRR");
            Rigidbody toyRb = other.GetComponent<Rigidbody>(); // Get the RB of the toy

            if (toyRb != null && !toyRb.isKinematic) // Check if the toy is being held currently (kinematic is set to true)
            {
                EventManager.TriggerToyPlacedOnPillar(other.gameObject, pillarNumber); // Snap the toy to the pillar
                isToyOnPillar = true;
                other.GetComponent<Collider>().enabled = false;
            }
       
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isToyOnPillar && other.gameObject.layer == LayerMask.NameToLayer("Toy"))
        {
            isToyOnPillar = false;
            other.GetComponent<Collider>().enabled = true;
        }
    }
}
