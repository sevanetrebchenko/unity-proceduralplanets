using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : iNoiseFilter 
{
    Noise noise = new Noise();
    NoiseSettings.SimpleNoiseSettings noiseSettings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        noiseSettings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = noiseSettings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < noiseSettings.noiseLayers; ++i)
        {
            float noiseIteration = noise.Evaluate(point * frequency + noiseSettings.center);
            noiseValue += (noiseIteration + 1) * 0.5f * amplitude;

            frequency *= noiseSettings.roughness;
            amplitude *= noiseSettings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - noiseSettings.minimum);
        return noiseValue * noiseSettings.strength;
    }
}
