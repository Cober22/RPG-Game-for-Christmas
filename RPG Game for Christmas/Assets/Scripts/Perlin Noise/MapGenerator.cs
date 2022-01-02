using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public float noiseScale;
	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
    public Vector2 offset;

    public bool autoUpdate;

	public TerrainType[] regions;
    public UnitType[] unitsCollection;

    private int mapWidth;
	private int mapHeight;

    private Nodo[,] grid;

    [SerializeField]
    float _decay = 0.3f;

    [SerializeField]
    float _momentum = 0.8f;

    [SerializeField]
    int _updateFrequency = 3;


    private void Start()
    {
        grid = GetComponent<Grid>().grid;
        mapWidth = GetComponent<Grid>().gridSizeX;
        mapHeight = GetComponent<Grid>().gridSizeY;

        GenerateMap();
    }

    public void GenerateMap()
    {
        seed = Random.Range(0, 100000);
        RestoreMap();
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
    }

    private void RestoreMap()
    {
        /* Initialice values */
    }

    void OnValidate() {
		if (mapWidth < 1) {
			mapWidth = 1;
		}
		if (mapHeight < 1) {
			mapHeight = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
    public GameObject element;
}

[System.Serializable]
public struct UnitType
{
    public string name;
    [Range(1, 5)]
    public int cantidad;
    public GameObject unit;
}