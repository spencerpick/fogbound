using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{

    public TextMeshProUGUI currentThought; // Reference to the text component to be stored in here
    [SerializeField] private Light highlightLightPrefab; // Reference to the prefab of the highlight object to be stored in here

    private Dictionary<GameObject, Light> activeHighlights = new Dictionary<GameObject, Light>(); // Dictionary will store active highlights in it, i.e. objects currently highlighted (making it easy to remove them)

    void Start()
    {
        /*if (currentThought == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                currentThought = canvas.GetComponent<TextMeshProUGUI>();
                Debug.Log("Assigned currentThought: " + (currentThought != null ? "Success" : "Failed"));
            }

        }*/
        Debug.Log("Assigned currentThought: " + (currentThought != null ? "Success" : "Failed"));
    }

    void OnEnable()
    {
        EventManager.OnThoughtUpdate += UpdateThought;
        EventManager.OnHighlightObject += HandleHighlight;
    }

    void OnDisable()
    {
        EventManager.OnThoughtUpdate -= UpdateThought;
        EventManager.OnHighlightObject -= HandleHighlight;
    }

    public void UpdateThought(string newThought)
    {
        currentThought.text = newThought;
    }

    private void HandleHighlight(GameObject obj, float intensity, float range, bool highlight, float yPosOffset = 0f)
    {
        Debug.Log("HIGHLIGHTING" + obj + " " + highlight);
        if (highlight)
        {
            if (!activeHighlights.ContainsKey(obj))
            {
                // Highlight the object with a green light
                Light highlightLight = Instantiate(highlightLightPrefab, obj.transform);
                // Set the local position to (0, 0, 0) so that it spawns at the object's position
                highlightLight.transform.localPosition = new Vector3(0f, yPosOffset, 0f);
                highlightLight.color = Color.green;
                highlightLight.range = 5f;
                highlightLight.intensity = 3f;

                // Store the reference of the object and the light together in the dictionary to make it easy to get rid of later
                activeHighlights[obj] = highlightLight;
            }
        }
        else // remove the highlight
        {
            // If the highlight exists then remove it
            if (activeHighlights.ContainsKey(obj))
            {
                Destroy(activeHighlights[obj].gameObject); // Destroy the light
                activeHighlights.Remove(obj); // Remove it from the active highlights dictionary as it is no longer an active highlight
            }
        }
    }
}
