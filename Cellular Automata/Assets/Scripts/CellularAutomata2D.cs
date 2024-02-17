using System.Collections;
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
    private int m_overpopulationLimit = 3;
    private int m_iterations = 2;
    private bool m_isInstant;
    private int m_gridWidth = 50;
    private int m_gridHeight = 50;
    private int m_neighbourCellsToBeAlive = 3;
    private int m_neighbourCellsToBeDead = 2;
    private GameObject[,] m_tilesInWorld;

    /// <summary>
    /// Generates a new 2D cellular automata map based on specified parameters.
    /// </summary>
    public void GenerateAutomata() {
        m_gridWidth = int.Parse(widthInput.text);
        m_gridHeight = int.Parse(heightInput.text);
        m_neighbourCellsToBeAlive = int.Parse(iterationsInput.text);
        m_neighbourCellsToBeDead = int.Parse(cellsToBeDeadInput.text);
        m_iterations = int.Parse(iterationsInput.text);
        m_overpopulationLimit = int.Parse(overpopulationInput.text);
        m_isInstant = !isInstantToggle.isOn;
        GenerateRandomMap();
        drawTiles();
        if (m_isInstant) {
            generateMapInSceneNoDelay();
        } else {
            StartCoroutine(generateMapInScene());
        }
    }

    private void drawTiles() {
        m_tilesInWorld = new GameObject[m_gridWidth, m_gridHeight];
        for (int i = 0; i < m_gridWidth; i++) {
            for (int j = 0; j < m_gridHeight; j++) {
                if (m_tilesInWorld[i, j] == null) {
                    Vector2 spawnPos = new Vector2(i, -j);
                    GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                    m_tilesInWorld[i, j] = tile;
                }
                GameObject tileObj = m_tilesInWorld[i, j];
                tileObj.GetComponent<SpriteRenderer>().color = m_map1[i, j] ? paintedTileColor : Color.white;
            }
        }
    }

    /// <summary>
    /// Generates a random initial map for the cellular automata based on specified parameters.
    /// </summary>
    private void GenerateRandomMap() {
        m_map1 = new bool[m_gridWidth, m_gridHeight]; // x, y
        for (int y = 0; y < m_gridHeight; y++) {
            for (int x = 0; x < m_gridWidth; x++) {
                m_map1[y, x] = Random.value < cubeProbability; // True = wall, false = floor;
            }
        }
    }

    private void iterateNewMap() {
        bool[,] tempMap = new bool[m_gridWidth, m_gridHeight];
        for (int y = 0; y < m_gridHeight; y++) {
            for (int x = 0; x < m_gridWidth; x++) {
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
        if (x < 0 || x >= m_gridWidth || y < 0 || y >= m_gridHeight) {
            return true;
        }
        return m_map1[x, y];
    }

    /// <summary>
    /// Generates the cellular automata map and visualizes it in the scene with a delay between iterations.
    /// </summary>
    /// <returns>Coroutine for delayed map generation.</returns>
    private IEnumerator generateMapInScene() {
        for (int i = 0; i <= m_iterations; i++) {
            drawTiles();
            yield return new WaitForSeconds(0.1f);
            iterateNewMap();
        }
    }

    /// <summary>
    /// Generates the cellular automata map and visualizes it in the scene without any delay between iterations.
    /// </summary>
    private void generateMapInSceneNoDelay() {
        for (int i = 0; i <= m_iterations; i++) {
            drawTiles();
            iterateNewMap();
        }
    }
}