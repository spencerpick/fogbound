﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveObjectManager : MonoBehaviour
{
    public Image cursorImage;
    public Rigidbody playerRigidbody;

    public GameObject keyDisplay;

    public GameObject iconList;
    public GameObject iconPrefab;
    public KeyCode interactKey = KeyCode.E;
    public Text KeyText;

    private GameObject pickupableItem;
    private float pickedUpItemNum = 0;
    private List<GameObject> itemList = new List<GameObject>();

    private Color materialColor;

    private Camera playerCamera;
    private GameObject grabbedObject;

    private FixedJoint joint;

    private readonly float RayCastDelay = 0.2f;
    private readonly float RayCastDistance = 7;

    private Vector3 startingCursorScale = Vector3.zero;
    private Vector3 cursorScaleMax = Vector3.one;
    private bool isInteractiveInView = false;
    private bool canRelease = true;

    private List<GameObject> InteractiveObjects;

    void Start()
    {
        playerCamera = Camera.main;

        StartCoroutine(CheckHighlight());

        // Get all of the "Interactive" objects
        InteractiveObjects = FindObjectsByTag("Interactive");

        if (InteractiveObjects != null)
        {
            // Loop through all triggerObjects
            foreach (GameObject triggerObject in InteractiveObjects)
            {
                TriggerHandler triggerHandler = triggerObject.GetComponent<TriggerHandler>();

                // Add a TriggerHandler if there isn't one already
                if (triggerHandler == null)
                {
                    triggerHandler = triggerObject.AddComponent<TriggerHandler>();
                }

                triggerHandler.OnEnter += HandleTriggerEnter;
                triggerHandler.OnExit += HandleTriggerExit;

                Rigidbody rbody = triggerObject.GetComponent<Rigidbody>();

                // Add a RigidBody is there isn't one already
                if (rbody == null)
                {
                    rbody = triggerObject.AddComponent<Rigidbody>();
                }
            }
        }

        // Initiate cursor scaling
        startingCursorScale = cursorImage.transform.localScale;
        cursorScaleMax = startingCursorScale * 1.5f;

        // Set the interact key info
        KeyText.text = interactKey.ToString();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Mouse pressed
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, RayCastDistance))
            {
                if (hit.collider.CompareTag("Interactive") && grabbedObject == null)
                {
                    grabbedObject = hit.collider.gameObject;
                    AttachObject(playerRigidbody);
                }
            }
        }

        // Release the object on mouse release
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }

        if (Input.GetKeyDown(interactKey) && pickupableItem != null)
        {
            PickupItem(pickupableItem);
            keyDisplay.SetActive(false);
        }
    }

    private void AttachObject(Rigidbody rb)
    {
        if (rb != null && joint == null)
        {
            // Freeze the rotation 
            grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            // Connect with the joint
            joint = grabbedObject.AddComponent<FixedJoint>();
            joint.connectedBody = rb;

            // Get the material colora
            Renderer renderer = grabbedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                materialColor = renderer.material.color;
            }

            grabbedObject.GetComponent<Collider>().isTrigger = true;
        }
    }

    private List<GameObject> FindObjectsByTag(string tag)
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(tag);
        List<GameObject> taggedObjects = new List<GameObject>(allObjects);
        return taggedObjects;
    }

    private void HandleTriggerEnter(Collider other)
    {
        if (other.name != "Flashlight")
        {
            canRelease = false;
            SetMaterialTransparency(0.5f, Color.red);
        }
    }

    private void HandleTriggerExit(Collider other)
    {
        canRelease = true;
        SetMaterialTransparency(1f, materialColor);
    }

    private void ReleaseObject()
    {
        if (joint != null && canRelease)
        {
            Destroy(joint); // Remove the joint from the object

            // Unfreeze the rotation 
            grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            // Reset collider and layer
            grabbedObject.GetComponent<Collider>().isTrigger = false;

            // Set the material back to opaque when released
            SetMaterialTransparency(1f, materialColor);

            joint = null;
            grabbedObject = null;
        }
    }

    private void SetMaterialTransparency(float alpha, Color C)
    {
        if (grabbedObject == null)
        {
            return;
        }
        Renderer renderer = grabbedObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color color = C;
            Material material = renderer.material;

            // Set the color with the new alpha value
            color.a = alpha;
            material.color = color;

            if (alpha < 1f)
            {
                // Configure shader for transparency
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
            else
            {
                // Configure shader for opaque
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
            }
        }
    }

    /* Cursor Animation */

    IEnumerator CheckHighlight()
    {
        while (true)
        {
            yield return new WaitForSeconds(RayCastDelay);
            PerformRaycast();
            AnimateCursor();
        }
    }

    void PerformRaycast()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, RayCastDistance))
        {
            if (hit.collider.CompareTag("PickupItem"))
            {
                keyDisplay.SetActive(true);
                pickupableItem = hit.collider.gameObject;
            }
            else
            {
                pickupableItem = null;
                keyDisplay.SetActive(false);
            }

            if (!hit.collider.CompareTag("Interactive") && !hit.collider.CompareTag("PickupItem"))
            {
                isInteractiveInView = false;
                return;
            }

            // The Raycast hit something
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null)
            {
                isInteractiveInView = true;
            }
        }
        else
        {
            isInteractiveInView = false;
            keyDisplay.SetActive(false);
        }
    }

    void AnimateCursor()
    {
        if (isInteractiveInView && grabbedObject == null)
        {
            // Scale up
            cursorImage.transform.localScale = cursorScaleMax;
        }
        else
        {
            // Scale down
            cursorImage.transform.localScale = startingCursorScale;
        }
    }


    void PickupItem(GameObject item)
    {
        pickedUpItemNum++;

        // Get the item icon from the pickup item
        Sprite itemIcon = item.GetComponent<PickupItem>().itemIcon;
        if (!itemIcon)
        {
            Debug.Log(item.name + " has no icon!");
            return;
        }

        // Create and display the new icon
        GameObject newIcon = Instantiate(iconPrefab);
        newIcon.GetComponent<Image>().sprite = itemIcon;
        newIcon.transform.SetParent(iconList.transform);

        // Offset the new icon
        float offsetX = 130f;
        newIcon.transform.localPosition = new Vector3(pickedUpItemNum * offsetX, 0f, 0f);

        // Reset rotation and scale
        newIcon.transform.localRotation = Quaternion.identity;
        newIcon.transform.localScale = Vector3.one;

        itemList.Add(newIcon);
        GameObject.Destroy(item);
    }
}