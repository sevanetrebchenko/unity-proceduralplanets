using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static iNoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        switch (settings.filterType)
        {
            case NoiseSettings.FilterType.SIMPLE:
                return new SimpleNoiseFilter(settings.simpleNoiseSettings);
            case NoiseSettings.FilterType.RIGID:
                return new RigidNoiseFilter(settings.rigidNoiseSettings);
        }

        return null; // Should never happen.
    }
}
