using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public float strength = 1;
    [Range(1,8)]
    public int noiseLayers = 1;
    public float baseRoughness = 1;
    public float roughness = 2;
    public float persistence = 0.5f; // Amplitude of noise multiplied by this value on each iteration
                                     // Higher frequencies of noise should have a smaller effect on the overall planet shape.
    public Vector3 center;

    public float minimum;
}
