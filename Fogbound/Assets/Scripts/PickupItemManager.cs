using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemManager : MonoBehaviour
{
    public GameObject keyDisplay;

    private Camera playerCamera;

    private readonly float RayCastDelay = 0.1f;
    private readonly float RayCastDistance = 7;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        Debug.Log(playerCamera);


        StartCoroutine(CheckHighlight());
    }

    IEnumerator CheckHighlight()
    {
        while (true)
        {
            yield return new WaitForSeconds(RayCastDelay);
            PerformRaycast();
        }
    }

    void PerformRaycast()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, RayCastDistance))
        {
            if (hit.collider.CompareTag("PickupItem"))
            {
                keyDisplay.SetActive(true);
                return;
            }
            else
            {
                keyDisplay.SetActive(false);
            }
        }
    }
}
