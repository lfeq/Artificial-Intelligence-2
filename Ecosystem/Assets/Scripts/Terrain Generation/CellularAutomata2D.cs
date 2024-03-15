using System.Collections;
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
    private List<GameObject> walkableTiles = new List<GameObject>();

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
        GenerateRandomMap();
        generateMapInSceneNoDelay();
        setTiles();
        LevelManager.s_instance.setLevelState(LevelState.Menu);
    }

    public Transform getRandomWalkableTileTransform() {
        return walkableTiles[Random.Range(0, walkableTiles.Count)].transform;
    }

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
                int wallsArround = numWallsAroundTile(i, j);
                if (tileType == TileType.Grass && wallsArround != 9) {
                    tileController.setTileType(TileType.Earth);
                }
                tileController.setEarthTileAroundTile(wallsArround);
                tileObj.isStatic = true;
                if (tileType == TileType.Grass) {
                    walkableTiles.Add(tileObj);
                }
            }
        }
    }

    /// <summary>
    /// Generates a random initial map for the cellular automata based on specified parameters.
    /// </summary>
    private void GenerateRandomMap() {
        m_map1 = new bool[gridWidth, gridHeight]; // x, y
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                m_map1[y, x] = Random.value < cubeProbability; // true = grass, false = water
            }
        }
    }

    private void iterateNewMap() {
        bool[,] tempMap = new bool[gridWidth, gridHeight];
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                int num = numWallsAroundTile(x, y);
                tempMap[x, y] = num >= 5;
            }
        }
        m_map1 = tempMap;
    }

    private int numWallsAroundTile(int t_xPosition, int t_yPosition) {
        int numWalls = 0; // Total number of walls around current tile
        for (int y = t_yPosition - 1; y <= t_yPosition + 1; y++) {
            for (int x = t_xPosition - 1; x <= t_xPosition + 1; x++) {
                if (isSolid(x, y)) {
                    numWalls++;
                }
            }
        }
        return numWalls;
    }

    private bool isSolid(int x, int y) {
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