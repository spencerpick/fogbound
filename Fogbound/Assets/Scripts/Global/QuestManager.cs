using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{

    public GameObject Puzzle_1;
    public GameObject Puzzle_2;
    public GameObject Puzzle_3;
    public GameObject Final_Task;

    public TextMeshProUGUI currentThoughtText; // Reference to the text component to be stored in here
    [SerializeField] private Light highlightLightPrefab; // Reference to the prefab of the highlight object to be stored in here
    [SerializeField] private GameObject player; // Reference to the player to be stored in here


    private List<string> QuestStages;
    private string currentQuestStage;
    private int currentQuestNum = 0;

    private AudioSource audioSource; // Reference to the audio source
    private bool thoughtSoundPlayed = false;
    [SerializeField] private AudioClip thinkingSound;
    [SerializeField] private AudioClip gaspSound;

    private Dictionary<GameObject, Light> activeHighlights = new Dictionary<GameObject, Light>(); // Dictionary will store active highlights in it, i.e. objects currently highlighted (making it easy to remove them)


    /// Quest markers ///
    [SerializeField] private GameObject First_Ghost_Marker;
    [SerializeField] private GameObject Orphanage_Entrance_Marker;
    [SerializeField] private GameObject Orphanage_Look_Around_Marker_1;
    [SerializeField] private GameObject Orphanage_Look_Around_Marker_2;
    [SerializeField] private GameObject Pillar_1_Marker;
    [SerializeField] private GameObject Pillar_2_Marker;
    [SerializeField] private GameObject Pillar_3_Marker;
    [SerializeField] private GameObject Pillar_4_Marker;
    [SerializeField] private GameObject Pillar_5_Marker;
    [SerializeField] private GameObject Pillar_6_Marker;
    [SerializeField] private GameObject Puzzle_1_Complete_Marker;

    [SerializeField] private GameObject Alter;
    [SerializeField] private GameObject FirstTargetObject;
    [SerializeField] private GameObject Puzzle2Location;
    [SerializeField] private GameObject Puzzle2Clues;


    [SerializeField] private GameObject Puzzle2RewardLocation;
    [SerializeField] private GameObject Puzzle3Location;
    [SerializeField] private GameObject WeightLocation;
    [SerializeField] private GameObject PressurePlate_1;
    [SerializeField] private GameObject PressurePlate_2;
    [SerializeField] private GameObject PressurePlate_3;
    [SerializeField] private GameObject PressurePlate_4;
    [SerializeField] private GameObject Puzzle_3Door;
    [SerializeField] private GameObject Puzzle3RewardLocation;

    void Start()
    {
        Debug.Log("STARTING QUEST MANAGER");
        QuestStages = new List<string>();

        // Initalise the different quest stages
        QuestStages.Add("First-Ghost");
        QuestStages.Add("Find-Orphanage");
        QuestStages.Add("Look-Around-Orphanage");
        QuestStages.Add("Place-Toys");

        QuestStages.Add("Examine Altar");
        QuestStages.Add("Find first object");
        QuestStages.Add("Puzzle 2 location");
        QuestStages.Add("Locate clues");
        QuestStages.Add("Take dropped item1 to alter");
        QuestStages.Add("Find puzzle 3");
        QuestStages.Add("Analyze puzzle 3");
        QuestStages.Add("Locate dropped item");
        QuestStages.Add("Take dropped item to alter");

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
        currentQuestStage = QuestStages[currentQuestNum]; 
    }

    private void HandleQuestStage()
    {
        if (currentQuestStage == "First-Ghost") // Quest stage 0
        {
            if(!thoughtSoundPlayed)
            {
                UpdateThought("My god, the rumours are true that's a ghost! How am I going to get past there is no other way?! Hmm what was that I heard about UV light stunning the paranormal? \n\n<b>PRESS x to change flashlight mode</b> ");
                PlayThoughtSound("gasp");
                thoughtSoundPlayed = true;
            }
            HandleHighlight(First_Ghost_Marker.gameObject, 6f, 5f, true, 1.31f);

            if(CheckDistToMarker(First_Ghost_Marker))
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = false;
                HandleHighlight(First_Ghost_Marker.gameObject, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Find-Orphanage") // Quest stage 1
        {
            UpdateThought("I should conserve my UV power it won't last long.. I need to find Emily quick! I should start by finding the orphanage.");
            if (!thoughtSoundPlayed)
            {
                PlayThoughtSound("thinking");
                thoughtSoundPlayed = true;
            }
            HandleHighlight(Orphanage_Entrance_Marker.gameObject, 6f, 5f, true, 1.31f);

            if (CheckDistToMarker(Orphanage_Entrance_Marker))
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = false;
                HandleHighlight(Orphanage_Entrance_Marker.gameObject, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Look-Around-Orphanage") // Quest stage 1
        {
            UpdateThought("I should look around the orphanage, this is where those other children went missing, maybe theres a clue about Emily..");
            if (!thoughtSoundPlayed)
            {
                PlayThoughtSound("thinking");
                thoughtSoundPlayed = true;
            }
            HandleHighlight(Orphanage_Look_Around_Marker_1.gameObject, 6f, 5f, true, 1.31f);
            HandleHighlight(Orphanage_Look_Around_Marker_2.gameObject, 6f, 5f, true, 1.31f);

            if (CheckDistToMarker(Orphanage_Look_Around_Marker_1) || CheckDistToMarker(Orphanage_Look_Around_Marker_2))
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = false;
                HandleHighlight(Orphanage_Look_Around_Marker_1.gameObject, 6f, 5f, false, 1.31f);
                HandleHighlight(Orphanage_Look_Around_Marker_2.gameObject, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Place-Toys") // Quest stage 1
        {
            UpdateThought("These pillars don't look like they belong here... They've got to be important somehow, hmm theres a toy on that one, maybe I should see if there are any other toys for the other pillars..\n\n<b>LMB while looking at a toy to pick it up</b>");
            if (!thoughtSoundPlayed)
            {
                PlayThoughtSound("gasp");
                thoughtSoundPlayed = true;
            }

            HandleHighlight(Pillar_1_Marker.gameObject, 6f, 5f, true, 1.31f);
            HandleHighlight(Pillar_2_Marker.gameObject, 6f, 5f, true, 1.31f);
            HandleHighlight(Pillar_3_Marker.gameObject, 6f, 5f, true, 1.31f);
            HandleHighlight(Pillar_4_Marker.gameObject, 6f, 5f, true, 1.31f);
            HandleHighlight(Pillar_5_Marker.gameObject, 6f, 5f, true, 1.31f);
            HandleHighlight(Pillar_6_Marker.gameObject, 6f, 5f, true, 1.31f);
            HandleHighlight(Puzzle_1_Complete_Marker.gameObject, 6f, 3f, true, 1.31f);
            if(Puzzle_1.GetComponent<Puzzle_1>().isComplete){
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = false;

                HandleHighlight(Pillar_1_Marker.gameObject, 6f, 5f, false, 1.31f);
                HandleHighlight(Pillar_2_Marker.gameObject, 6f, 5f, false, 1.31f);
                HandleHighlight(Pillar_3_Marker.gameObject, 6f, 5f, false, 1.31f);
                HandleHighlight(Pillar_4_Marker.gameObject, 6f, 5f, false, 1.31f);
                HandleHighlight(Pillar_5_Marker.gameObject, 6f, 5f, false, 1.31f);
                HandleHighlight(Pillar_6_Marker.gameObject, 6f, 5f, false, 1.31f);
                HandleHighlight(Puzzle_1_Complete_Marker.gameObject, 6f, 3f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Examine Altar")
        {
            UpdateThought("There's something strange about this altar... It feels like it's connected to everything going on here. I should take a closer look");

            HandleHighlight(Alter, 6f, 5f, true, 1.31f);

            if (CheckDistToMarker(Alter))
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = true;

                HandleHighlight(Alter, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Find first object")
        {
            UpdateThought("If the altar is important, then there must be something to place on it. I need to find them.");

            HandleHighlight(FirstTargetObject, 6f, 5f, true, 1.31f);

            if (CheckDistToMarker(FirstTargetObject))
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = true;

                HandleHighlight(FirstTargetObject, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Puzzle 2 location")
        {
            UpdateThought("On my way here, I noticed a strange glow coming from down the street. I wonder if it has something to do with this altar.");

            HandleHighlight(Puzzle2Location, 6f, 5f, true, 1.31f);

            if (CheckDistToMarker(Puzzle2Location))
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = true;
                PlayThoughtSound("gasp");

                HandleHighlight(Puzzle2Location, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Locate clues")
        {
            UpdateThought("There have to be clues around here to help solve this. I should look carefully for anything that might give me a hint.");

            HandleHighlight(Puzzle2Clues, 6f, 5f, true, 1.31f);

            if (Puzzle_2.GetComponent<Puzzle_2>().puzzleCompleted)
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = false;
                HandleHighlight(Puzzle2Clues, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Take dropped item1 to alter")
        {
            UpdateThought("Look! This seems important, I should take it to the altar and see what happens");

            HandleHighlight(Alter, 6f, 5f, true, 1.31f);

            if (Alter.GetComponent<DetectFinalItems>().objectCount == 2)
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = true;

                HandleHighlight(Alter, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Find puzzle 3")
        {
            UpdateThought("There were odd looking shapes near Auto Parts. I should check them out.");

            HandleHighlight(WeightLocation, 6f, 5f, true, 1.31f);

            if (CheckDistToMarker(WeightLocation))
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = true;

                HandleHighlight(WeightLocation, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Analyze puzzle 3")
        {
            UpdateThought("Interesting... Numbered plates. I wonder what they go to.");

            // Highlight pressure plates
            HandleHighlight(PressurePlate_1, 6f, 5f, true, 1.31f);
            HandleHighlight(PressurePlate_2, 6f, 5f, true, 1.31f);
            HandleHighlight(PressurePlate_3, 6f, 5f, true, 1.31f);
            HandleHighlight(PressurePlate_4, 6f, 5f, true, 1.31f);
            HandleHighlight(Puzzle_3Door, 6f, 5f, true, 1.31f);

            if (Puzzle_3.GetComponent<Puzzle_3>().puzzleCompleted){
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = true;

                HandleHighlight(PressurePlate_1, 6f, 5f, false, 1.31f);
                HandleHighlight(PressurePlate_2, 6f, 5f, false, 1.31f);
                HandleHighlight(PressurePlate_3, 6f, 5f, false, 1.31f);
                HandleHighlight(PressurePlate_4, 6f, 5f, false, 1.31f);
                HandleHighlight(Puzzle_3Door, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Locate dropped item")
        {
            UpdateThought("I heard something drop after solving the puzzle. That must be the next item I need.");

            HandleHighlight(Puzzle3RewardLocation, 6f, 5f, true, 1.31f);

            if (CheckDistToMarker(Puzzle3RewardLocation))
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = true;

                HandleHighlight(Puzzle3RewardLocation, 6f, 5f, false, 1.31f);
            }
        }

        if (currentQuestStage == "Take dropped item to alter")
        {
            UpdateThought("This must be it, the final item. I need to place it on the altar and see what happens next.");

            HandleHighlight(Alter, 6f, 5f, true, 1.31f);

            /*if (Alter.GetComponent<DetectFinalItems>().objectCount == 3)
            {
                currentQuestNum += 1;
                currentQuestStage = QuestStages[currentQuestNum];
                thoughtSoundPlayed = true;

                HandleHighlight(Puzzle3RewardLocation, 6f, 5f, false, 1.31f);
            }*/
        }

    }

    private bool CheckDistToMarker(GameObject marker)
    {
        float distance = Vector3.Distance(player.transform.position, marker.transform.position);

        if (distance <= 5f)
        {
            return true;
        }

        return false;
    }
        
    private void PlayThoughtSound(string soundType)
    {
        if (!audioSource.isPlaying)
        {
            switch (soundType)
            {
                case "thinking":
                    audioSource.clip = thinkingSound;
                    break;
                case "gasp":
                    audioSource.clip = gaspSound;
                    break;
                default:
                    audioSource.clip = thinkingSound; // Default to thinking sound if nothing is passed
                    break;
            }
            audioSource.Play();
        }
    }

    public void UpdateThought(string newThought)
    {
        currentThoughtText.text = newThought;
    }

    private void HandleHighlight(GameObject obj, float intensity, float range, bool highlight, float yPosOffset = 0f)
    {
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
