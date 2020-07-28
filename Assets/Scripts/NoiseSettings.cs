using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        SIMPLE,
        RIGID
    }
    public FilterType filterType;

    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1;
        [Range(1, 8)]
        public int noiseLayers = 1;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = 0.5f; // Amplitude of noise multiplied by this value on each iteration
                                         // Higher frequencies of noise should have a smaller effect on the overall planet shape.
        public Vector3 center;
        public float minimum;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = 0.8f;

    }


}
