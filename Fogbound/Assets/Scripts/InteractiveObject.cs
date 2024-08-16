using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    public Image cursorImageCenter;
    public Image cursorImageBorder;

    public Rigidbody playerRigidbody;

    public Material yesDropMaterial;
    public Material noDropMaterial;
    private Color materialColor;

    private Camera playerCamera;
    private GameObject grabbedObject;

    private FixedJoint joint;

    private readonly float RayCastDelay = 0.1f;
    private readonly float RayCastDistance = 5;

    private float cursorScale = 1f;
    private readonly float cursorScaleMax = 1.5f;
    private bool isInteractiveInView = false;
    private bool canRelease = true;
    private Vector3 startingScale = Vector3.zero;

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
            }
        }
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
                    AttachObject(playerCamera.GetComponent<Rigidbody>());
                }
            }
        }

        // Release the object on mouse release
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
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

            // Change the layer to a non player colliding layer
            //grabbedObject.tag = "NonColliding";// LayerMask.NameToLayer("NonColliding");
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
        canRelease = false;
        SetMaterialTransparency(0.5f, Color.red);
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
            //grabbedObject.tag = "Interactive";//LayerMask.NameToLayer("Default");

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
            //AnimateCursorBorder();
        }
    }

    void PerformRaycast()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, RayCastDistance))
        {
            if (!hit.collider.CompareTag("Interactive"))
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
        }
    }

    void AnimateCursorBorder()
    {
        if (isInteractiveInView && grabbedObject == null)
        {
            // Scale up
            cursorScale = cursorScaleMax;
        }
        else
        {
            // Scale down
            cursorScale = 1f;
        }

        // Apply the scale
        cursorImageBorder.rectTransform.localScale = new Vector3(cursorScale, cursorScale, 1);
    }
}