using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the generation and visualization of 2D cellular automata maps.
/// </summary>
public class CellularAutomata2D : MonoBehaviour {
    public static CellularAutomata2D s_instance;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField, Range(0.01f, 1)] private float cubeProbability = 0.5f;
    [SerializeField] private int iterations = 5;
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;

    private bool[,] m_map1;
    private GameObject[,] m_tilesInWorld;
    private List<GameObject> m_walkableTiles = new List<GameObject>();
    private const int GrassDensityLimit = 5;

    private void Awake() {
        if (FindObjectOfType<CellularAutomata2D>() != null &&
            FindObjectOfType<CellularAutomata2D>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
    }

    /// <summary>
    /// Generates a new 2D cellular automata map based on specified parameters.
    /// </summary>
    public void generateAutomata() {
        generateRandomMap();
        generateMapInSceneNoDelay();
        setTiles();
        LevelManager.s_instance.setLevelState(LevelState.Menu);
    }

    /// <summary>
    /// Gets a random walkable tile's Transform.
    /// </summary>
    /// <returns>Transform of a random walkable tile.</returns>
    public Transform getRandomWalkableTileTransform() {
        return m_walkableTiles[Random.Range(0, m_walkableTiles.Count)].transform;
    }

    /// <summary>
    /// Draws tiles based on the generated map and sets their properties.
    /// </summary>
    private void drawTiles() {
        m_tilesInWorld = new GameObject[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++) {
            for (int j = 0; j < gridHeight; j++) {
                if (m_tilesInWorld[i, j] == null) {
                    Vector3 spawnPos = new Vector3(i, 0, j);
                    GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                    tile.transform.parent = transform;
                    m_tilesInWorld[i, j] = tile;
                }
                GameObject tileObj = m_tilesInWorld[i, j];
                TileController tileController = tileObj.GetComponent<TileController>();
                // Set tile type
                TileType tileType = m_map1[i, j] ? TileType.Grass : TileType.Water;
                if (i == 0 || i == gridWidth - 1 || j == 0 || j == gridHeight - 1) {
                    tileType = TileType.Border;
                }
                tileController.setTileType(tileType);
                int wallsAround = numberOfGrassesAroundTile(i, j);
                if (tileType == TileType.Grass && wallsAround != 9) {
                    tileController.setTileType(TileType.Earth);
                }
                tileController.setEarthTileAroundTile(wallsAround);
                tileObj.isStatic = true;
                if (tileType == TileType.Grass) {
                    m_walkableTiles.Add(tileObj);
                }
            }
        }
    }

    /// <summary>
    /// Generates a random initial map for the cellular automata based on specified parameters.
    /// </summary>
    private void generateRandomMap() {
        m_map1 = new bool[gridWidth, gridHeight]; // x, y
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                m_map1[y, x] = Random.value < cubeProbability; // true = grass, false = water
            }
        }
    }

    /// <summary>
    /// Iterates over the map to generate a new map based on certain rules.
    /// </summary>
    private void iterateNewMap() {
        bool[,] tempMap = new bool[gridWidth, gridHeight];
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                int numberOfGrasses = numberOfGrassesAroundTile(x, y);
                tempMap[x, y] = numberOfGrasses >= GrassDensityLimit;
            }
        }
        m_map1 = tempMap;
    }

    /// <summary>
    /// Counts the number of grasses around a tile position.
    /// </summary>
    /// <param name="tXPosition">X position of the tile.</param>
    /// <param name="tYPosition">Y position of the tile.</param>
    /// <returns>The number of grasses around the tile position.</returns>
    private int numberOfGrassesAroundTile(int tXPosition, int tYPosition) {
        int numberOfGrasses = 0; // Total number of grasses around current tile
        for (int y = tYPosition - 1; y <= tYPosition + 1; y++) {
            for (int x = tXPosition - 1; x <= tXPosition + 1; x++) {
                if (isGrass(x, y)) {
                    numberOfGrasses++;
                }
            }
        }
        return numberOfGrasses;
    }

    /// <summary>
    /// Checks if the specified position is grass (either out of bounds or a solid tile).
    /// </summary>
    /// <param name="x">X position.</param>
    /// <param name="y">Y position.</param>
    /// <returns>True if the position is grass, false otherwise.</returns>
    private bool isGrass(int x, int y) {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight) {
            return true;
        }
        return m_map1[x, y];
    }

    /// <summary>
    /// Generates the cellular automata map and visualizes it in the scene without any delay between iterations.
    /// </summary>
    private void generateMapInSceneNoDelay() {
        for (int i = 0; i <= iterations; i++) {
            iterateNewMap();
        }
        drawTiles();
    }

    /// <summary>
    /// Sets up tiles after generating the map.
    /// </summary>
    private void setTiles() {
        for (int i = 0; i < gridWidth; i++) {
            for (int j = 0; j < gridHeight; j++) {
                GameObject tileObj = m_tilesInWorld[i, j];
                TileController tileController = tileObj.GetComponent<TileController>();
                tileController.setupTiles();
            }
        }
    }
}