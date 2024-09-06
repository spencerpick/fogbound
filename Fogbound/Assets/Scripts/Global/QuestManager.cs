using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{

    public TextMeshProUGUI currentThoughtText; // Reference to the text component to be stored in here
    [SerializeField] private Light highlightLightPrefab; // Reference to the prefab of the highlight object to be stored in here

    [SerializeField]
    private int currentThoughtIndex = 0;

    [SerializeField]
    private List<string> Thoughts;

    private Dictionary<GameObject, Light> activeHighlights = new Dictionary<GameObject, Light>(); // Dictionary will store active highlights in it, i.e. objects currently highlighted (making it easy to remove them)

    void Start()
    {
 
        // TODO Add guiding thoughts here
        Thoughts.Add("The orphanage. It all started here. There has to be something inside that will tell me what happened… and why.");

        Thoughts.Add("These pillars… six of them, and they’re missing something.");
        Thoughts.Add("A toy! I bet these toys are scattered throughout the orphanage.");
        Thoughts.Add("Keep an eye out, any room could hold one of them.");
        Thoughts.Add("I’ve got all the toys. Time to place them on the pillars. There has to be a right order.");
        Thoughts.Add("Let’s hope I’ve got the right combination. The wrong one might not end well.");
        Thoughts.Add("The pillars accepted the toys. Something shifted… I’m closer to understanding this place. But what else is lurking here?");

        Thoughts.Add("Looks like I’m not getting that 'itemname' unless I climb. These boxes should do the trick.");
        Thoughts.Add("I’m getting closer. The fog seems… angrier. Almost like it doesn’t want me to know the truth.");

        Thoughts.Add("Four towers… three switches on each. Something tells me this is more than just a guessing game.");
        Thoughts.Add("These switches move up and down. Random… or is there a hidden clue somewhere in this place?");
        Thoughts.Add("Might take a few tries to get this right. Each tower affects the others. Patience.");
        Thoughts.Add("There, that did it. The switches are aligned.");

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
        currentThoughtText.text = newThought;
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
