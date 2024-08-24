using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3; // Maximum number of lives
  
    public float heartSpacing = 10f; // Spacing between hearts

    private int currentLives;
    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        currentLives = maxLives;
        
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
        
            Debug.Log("Player lost a life! Lives remaining: " + currentLives);

            if (currentLives <= 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the level
    }
}
