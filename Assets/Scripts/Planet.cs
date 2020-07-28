using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Planet : MonoBehaviour
{
    // Planet definition.
    [Range(2, 256)]
    public int resolution = 10;
    [SerializeField]
    bool autoUpdate = true;

    public enum FaceRenderMask { 
        ALL, 
        TOP, 
        BOTTOM, 
        LEFT, 
        RIGHT, 
        FRONT, 
        BACK 
    }

    public FaceRenderMask faceRenderMask;

    // Planet shape.
    public ShapeSettings shapeSettings;
    [HideInInspector]
    public bool showShapeSettings;

    // Planet color.
    public ColorSettings colorSettings;
    [HideInInspector]
    public bool showColorSettings;


    // Generation.
    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        terrainFaces = new TerrainFace[6];

        // Face directions.
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; ++i)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject();
                meshObject.transform.parent = transform;

                // Initialize mesh filter.
                meshObject.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            // Initialize terrain face.
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);

            bool renderFace = faceRenderMask == FaceRenderMask.ALL || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    // Re-generate the entire planet from scratch.
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    // Only re-generate planet color.
    public void OnColorSettingsUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    // Only regenerate planet size.
    public void OnShapeSettingsUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; ++i)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colorGenerator.UpdateElevation(shapeGenerator.elevationRange);
    }

    void GenerateColors()
    {
        colorGenerator.UpdateColors();
    }
}
