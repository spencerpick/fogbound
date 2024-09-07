using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{

    public TextMeshProUGUI currentThoughtText; // Reference to the text component to be stored in here
    [SerializeField] private Light highlightLightPrefab; // Reference to the prefab of the highlight object to be stored in here


    private List<string> QuestStages;
    private string currentQuestStage;

    private AudioSource audioSource; // Reference to the audio source
    private bool thoughtSoundPlayed = false;

    private Dictionary<GameObject, Light> activeHighlights = new Dictionary<GameObject, Light>(); // Dictionary will store active highlights in it, i.e. objects currently highlighted (making it easy to remove them)


    /// Quest markers ///
    [SerializeField] private GameObject Orphanage_Entrance_Marker;



    void Start()
    {

        QuestStages = new List<string>();
        // Initalise the different quest stages
        QuestStages.Add("First-Ghost");
        QuestStages.Add("Find-Orphanage");
        QuestStages.Add("Look-Around-Orphanage");
        QuestStages.Add("Place-Toys");
        QuestStages.Add("Read-note");
        QuestStages.Add("Find-House");
        QuestStages.Add("Reveal-Basement");
        QuestStages.Add("Complete-Ritual");

        audioSource = GetComponent<AudioSource>();
        
        StartCoroutine(DelayQuestStart(0.5f)); // Delay the initial quest stage update by 2 seconds to avoid sound cut-off


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

    void Update()
    {
        HandleQuestStage();
        Debug.Log(currentQuestStage);
    }

    IEnumerator DelayQuestStart(float delayTime) // Coroutine to delay the quest start and avoid sound cutting off
    {
        yield return new WaitForSeconds(delayTime);
        currentQuestStage = QuestStages[0]; 
    }

    private void HandleQuestStage()
    {
        
        if(currentQuestStage == "Find-Orphanage")
        {
            UpdateThought("Hmm I think I should find the orphanage");
            if(!thoughtSoundPlayed)
            {
                PlayThoughtSound();
                thoughtSoundPlayed = true;
            }
            HandleHighlight(Orphanage_Entrance_Marker.gameObject, 6f, 5f, true, 1.31f);
        }


    }

    private void PlayThoughtSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play(); // Use the sound already set in the AudioSource component
        }

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
                highlightLight.range = range;
                highlightLight.intensity = intensity;

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
