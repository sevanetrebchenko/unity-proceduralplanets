using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();

            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        // Add button to generate planet.
        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdate, ref planet.showShapeSettings, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdate, ref planet.showColorSettings, ref colorEditor);
    }

    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool showMenu, ref Editor editor)
    {
        if (settings)
        {
            // Display menu or not.
            showMenu = EditorGUILayout.InspectorTitlebar(showMenu, settings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (showMenu)
                {
                    // Draw menu.
                    // Only create a new editor when necessary (not every frame).
                    // Created editor gets stored in member variables above.
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    // Invoke changes made by callback functions.
                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
