using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : iNoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.RigidNoiseSettings noiseSettings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        noiseSettings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = noiseSettings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < noiseSettings.noiseLayers; ++i)
        {
            float noiseIteration = 1 - Mathf.Abs(noise.Evaluate(point * frequency + noiseSettings.center));
            noiseIteration *= noiseIteration;

            // In each layer, we multiply the weight and set it for the next layer.
            // Regions that start out lower down remain undetailed in comparison to higher regions.
            noiseIteration *= weight;
            weight = Mathf.Clamp01(noiseIteration * noiseSettings.weightMultiplier);

            noiseValue += noiseIteration * amplitude;

            frequency *= noiseSettings.roughness;
            amplitude *= noiseSettings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - noiseSettings.minimum);
        return noiseValue * noiseSettings.strength;
    }
}
