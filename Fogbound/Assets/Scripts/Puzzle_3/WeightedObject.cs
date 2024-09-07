using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeightedObject : MonoBehaviour
{
    public float weight;

    private TextMeshPro[] weightTexts;

    private void Start()
    {
        weightTexts = GetComponentsInChildren<TextMeshPro>();

        foreach(var t in weightTexts)
        {
            t.text = weight.ToString();
        }
    }
}
