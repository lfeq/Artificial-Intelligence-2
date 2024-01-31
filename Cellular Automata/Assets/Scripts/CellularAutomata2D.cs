using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the generation and visualization of 2D cellular automata maps.
/// </summary>
public class CellularAutomata2D : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Color paintedTileColor = Color.red;
    [SerializeField, Range(0.01f, 1)] private float cubeProbability = 0.5f;
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;
    [SerializeField] private TMP_InputField iterationsInput;
    [SerializeField] private TMP_InputField cellsToBeAliveInput;
    [SerializeField] private TMP_InputField cellsToBeDeadInput;
    [SerializeField] private TMP_InputField overpopulationInput;
    [SerializeField] private Toggle isInstantToggle;

    private bool[,] m_map1; //false = alive, true = dead/painted
    private List<GameObject> m_tiles = new List<GameObject>();
    private int m_overpopulationLimit = 3;
    private int m_iterations = 2;
    private bool m_isInstant;
    private int m_gridWidth = 50;
    private int m_gridHeight = 50;
    private int m_neighbourCellsToBeAlive = 3;
    private int m_neighbourCellsToBeDead = 2;

    /// <summary>
    /// Generates a new 2D cellular automata map based on specified parameters.
    /// </summary>
    public void GenerateAutomata() {
        m_tiles.Clear();
        m_gridWidth = int.Parse(widthInput.text);
        m_gridHeight = int.Parse(heightInput.text);
        m_neighbourCellsToBeAlive = int.Parse(iterationsInput.text);
        m_neighbourCellsToBeDead = int.Parse(cellsToBeDeadInput.text);
        m_iterations = int.Parse(iterationsInput.text);
        m_overpopulationLimit = int.Parse(overpopulationInput.text);
        m_isInstant = !isInstantToggle.isOn;
        GenerateRandomMap();
        if (m_isInstant) {
            GenerateMapInSceneNoDelay();
        } else {
            StartCoroutine(GenerateMapInScene());
        }
    }

    /// <summary>
    /// Generates a random initial map for the cellular automata based on specified parameters.
    /// </summary>
    private void GenerateRandomMap() {
        m_map1 = new bool[m_gridHeight, m_gridWidth]; // y, x
        for (int y = 0; y < m_map1.GetLength(0); y++) {
            for (int x = 0; x < m_map1.GetLength(1); x++) {
                if ((y == 0) || (y == m_gridHeight - 1) ||
                    (x == 0) || (x == m_gridWidth - 1)) {
                    m_map1[y, x] = true;
                    continue;
                }
                if (Random.value >= cubeProbability) {
                    m_map1[y, x] = false;
                } else {
                    m_map1[y, x] = true;
                }
            }
        }
    }

    /// <summary>
    /// Iterates over the current map to generate the next iteration of the cellular automata.
    /// </summary>
    private void IterateNewMap() {
        bool[,] map2 = m_map1;
        for (int y = 0; y < map2.GetLength(0); y++) {
            for (int x = 0; x < map2.GetLength(1); x++) {
                if ((y == 0) || (y == m_gridHeight - 1) ||
                    (x == 0) || (x == m_gridWidth - 1)) {
                    map2[y, x] = true;
                    continue;
                }
                map2[y, x] = Checktiles(map2, x, y);
            }
        }
        m_map1 = map2;
    }

    /// <summary>
    /// Checks the neighboring tiles of a specific position in the map and determines its state.
    /// </summary>
    /// <param name="t_map">The map to be checked.</param>
    /// <param name="t_xPosition">X-coordinate of the position.</param>
    /// <param name="t_yPosition">Y-coordinate of the position.</param>
    /// <returns>True if the tile should be dead, false if alive.</returns>
    private bool Checktiles(bool[,] t_map, int t_xPosition, int t_yPosition) {
        int aliveNeighbourTiles = 0, deadNeighbourTiles = 0;
        for (int y = t_yPosition - 1; y <= t_yPosition + 1; y++) {
            for (int x = t_xPosition - 1; x <= t_xPosition + 1; x++) {
                if (y == t_yPosition || x == t_xPosition) {
                    continue;
                }
                if (t_map[y, x]) {
                    deadNeighbourTiles++;
                } else {
                    aliveNeighbourTiles++;
                }
            }
        }
        if (aliveNeighbourTiles == m_neighbourCellsToBeAlive) {
            return false;
        } else if (deadNeighbourTiles == m_neighbourCellsToBeDead) {
            return true;
        } else if (m_overpopulationLimit == deadNeighbourTiles) {
            return true;
        } else {
            return t_map[t_yPosition, t_xPosition];
        }
    }

    /// <summary>
    /// Generates the cellular automata map and visualizes it in the scene with a delay between iterations.
    /// </summary>
    /// <returns>Coroutine for delayed map generation.</returns>
    private IEnumerator GenerateMapInScene() {
        for (int i = 0; i <= m_iterations; i++) {
            for (int y = 0; y < m_map1.GetLength(0); y++) {
                for (int x = 0; x < m_map1.GetLength(1); x++) {
                    yield return new WaitForSeconds(0.005f);
                    Vector2 spawnPosition = new Vector2(x, -y);
                    GameObject tempTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                    SpriteRenderer spriteRenderer = tempTile.GetComponent<SpriteRenderer>();
                    if (m_map1[x, y]) {
                        spriteRenderer.color = paintedTileColor;
                    } else {
                        spriteRenderer.color = Color.white;
                    }
                }
            }
            IterateNewMap();
        }
    }

    /// <summary>
    /// Generates the cellular automata map and visualizes it in the scene without any delay between iterations.
    /// </summary>
    private void GenerateMapInSceneNoDelay() {
        for (int i = 0; i <= m_iterations; i++) {
            for (int y = 0; y < m_map1.GetLength(0); y++) {
                for (int x = 0; x < m_map1.GetLength(1); x++) {
                    Vector2 spawnPosition = new Vector2(x, -y);
                    GameObject tempTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                    m_tiles.Add(tempTile);
                    SpriteRenderer spriteRenderer = tempTile.GetComponent<SpriteRenderer>();
                    if (m_map1[x, y]) {
                        spriteRenderer.color = paintedTileColor;
                    } else {
                        spriteRenderer.color = Color.white;
                    }
                }
            }
            IterateNewMap();
        }
    }
}