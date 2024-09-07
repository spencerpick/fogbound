using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Ensures the struct is shown in the Inspector
public struct LightRange
{
    public int start;
    public int end;
}

public class PulsingLight : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public LightRange lightRange;

    private Light thisLight;
    
    // Start is called before the first frame update
    void Start()
    {
        thisLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        thisLight.range = Mathf.Lerp(lightRange.start, lightRange.end, (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2);
    }
}
