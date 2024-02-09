using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class for implementing a 3D cellular automata simulation.
/// </summary>
public class CellularAutomata3D : MonoBehaviour {
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private TMP_InputField maxLivesInput;
    [SerializeField] private TMP_InputField surivivalInput;
    [SerializeField] private TMP_InputField birthInput;
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;
    [SerializeField] private TMP_InputField depthInput;
    [SerializeField] private TMP_Dropdown countTypeDropdown;
    [SerializeField, Range(0.01f, 1f)] private float cubeProbability = 0.5f;

    private int[,,] m_map1; //0 = dead, 1..5 = lives
    private List<GameObject> m_tiles = new List<GameObject>();
    private int m_maxLives = 5;
    private int m_survival = 3;
    private int m_birth = 2;
    private int m_width = 25;
    private int m_height = 25;
    private int m_depth = 25;
    private bool m_isUsingMoore = true;

    /// <summary>
    /// Start the cellular automata simulation.
    /// </summary>
    public void StartAutomata() {
        m_maxLives = int.Parse(maxLivesInput.text);
        m_survival = int.Parse(surivivalInput.text);
        m_birth = int.Parse(birthInput.text);
        m_width = int.Parse(widthInput.text);
        m_height = int.Parse(heightInput.text);
        m_depth = int.Parse(depthInput.text);
        m_isUsingMoore = countTypeDropdown.value == 0;
        GenerateRandomMap();
        StartCoroutine(GenerateMap());
    }

    /// <summary>
    /// Generate a random initial map.
    /// </summary>
    private void GenerateRandomMap() {
        m_map1 = new int[m_height, m_width, m_depth]; // y, x, z
        for (int y = 0; y < m_map1.GetLength(0); y++) {
            for (int x = 0; x < m_map1.GetLength(1); x++) {
                for (int z = 0; z < m_map1.GetLength(2); z++) {
                    if ((y == 0) || (y == m_height - 1) ||
                        (x == 0) || (x == m_width - 1) ||
                        (z == 0) || (z == m_depth - 1)) {
                        m_map1[y, x, z] = 0;
                        continue;
                    }
                    if (Random.value <= cubeProbability) {
                        m_map1[y, x, z] = Random.Range(1, m_maxLives);
                    } else {
                        m_map1[y, x, z] = 0;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Generate the map over time.
    /// </summary>
    private IEnumerator GenerateMap() {
        while (true) {
            yield return new WaitForSeconds(5f);
            foreach (GameObject go in m_tiles) {
                Destroy(go);
            }
            m_tiles.Clear();
            IterateNewMap();
            for (int y = 0; y < m_map1.GetLength(0); y++) {
                for (int x = 0; x < m_map1.GetLength(1); x++) {
                    for (int z = 0; z < m_map1.GetLength(2); z++) {
                        Vector3 spawnPosition = new Vector3(x, -y, z);
                        GameObject tempTile = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
                        if (m_map1[x, y, z] <= 0) {
                            tempTile.SetActive(false);
                        } else {
                        }
                        m_tiles.Add(tempTile);
                        tempTile.name = m_map1[x, y, z].ToString();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Iterate and generate a new map based on the current map.
    /// </summary>
    private void IterateNewMap() {
        int[,,] map2 = m_map1;
        for (int y = 0; y < map2.GetLength(0); y++) {
            for (int x = 0; x < map2.GetLength(1); x++) {
                for (int z = 0; z < m_map1.GetLength(2); z++) {
                    if ((y == 0) || (y == m_height - 1) ||
                        (x == 0) || (x == m_width - 1) ||
                        (z == 0) || (z == m_depth - 1)) {
                        map2[y, x, z] = 0;
                        continue;
                    }
                    if (m_isUsingMoore) {
                        map2[y, x, z] = CheckTilesMoore(map2, x, y, z);
                    } else {
                        map2[y, x, z] = CheckTilesNeumann(map2, x, y, z);
                    }
                }
            }
        }
        m_map1 = map2;
    }

    // Best result: maxLives = 5, survival = 5, birth = 8, width, height, depth = 25, cubeProbability = .4
    /// <summary>
    /// Check neighboring cells for Moore neighborhood.
    /// </summary>
    private int CheckTilesMoore(int[,,] t_map, int t_xPosition, int t_yPosition, int t_zPosition) {
        int aliveNeighbourTiles = 0, deadNeighbourTiles = 0;
        for (int y = t_yPosition - 1; y <= t_yPosition + 1; y++) {
            for (int x = t_xPosition - 1; x <= t_xPosition + 1; x++) {
                for (int z = t_zPosition - 1; z <= t_zPosition + 1; z++) {
                    if (y == t_yPosition && x == t_xPosition && z == t_zPosition) {
                        continue;
                    }
                    if (t_map[y, x, z] <= 0) {
                        deadNeighbourTiles++;
                    } else {
                        aliveNeighbourTiles++;
                    }
                }
            }
        }
        if (aliveNeighbourTiles >= m_survival) {
            return t_map[t_xPosition, t_yPosition, t_zPosition]; // Keep alive
        } else if (deadNeighbourTiles == m_birth) {
            return Random.Range(1, m_maxLives); // Birth cell
        } else {
            return t_map[t_xPosition, t_yPosition, t_zPosition] - 1;
        }
    }

    /// <summary>
    /// Check neighboring cells for Neumann neighborhood.
    /// </summary>
    private int CheckTilesNeumann(int[,,] t_map, int t_xPosition, int t_yPosition, int t_zPosition) {
        int aliveNeighbourTiles = 0, deadNeighbourTiles = 0;
        for (int y = t_yPosition - 1; y <= t_yPosition + 1; y++) {
            for (int x = t_xPosition - 1; x <= t_xPosition + 1; x++) {
                for (int z = t_zPosition - 1; z <= t_zPosition + 1; z++) {
                    bool center = (y == t_yPosition && x == t_xPosition && z == t_zPosition);
                    bool corner1 = (y == t_yPosition - 1 && x == t_xPosition - 1 && z == t_zPosition - 1);
                    bool corner2 = (y == t_yPosition - 1 && x == t_xPosition + 1 && z == t_zPosition - 1);
                    bool corner3 = (y == t_yPosition + 1 && x == t_xPosition - 1 && z == t_zPosition - 1);
                    bool corner4 = (y == t_yPosition + 1 && x == t_xPosition + 1 && z == t_zPosition - 1);
                    bool corner5 = (y == t_yPosition - 1 && x == t_xPosition - 1 && z == t_zPosition + 1);
                    bool corner6 = (y == t_yPosition - 1 && x == t_xPosition + 1 && z == t_zPosition + 1);
                    bool corner7 = (y == t_yPosition + 1 && x == t_xPosition - 1 && z == t_zPosition + 1);
                    bool corner8 = (y == t_yPosition + 1 && x == t_xPosition + 1 && z == t_zPosition + 1);
                    if (center || corner1 || corner2 || corner3 || corner4 || corner5 || corner6 || corner7 || corner8) { // Exclude corners and center cell
                        continue;
                    }
                    if (t_map[y, x, z] <= 0) {
                        deadNeighbourTiles++;
                    } else {
                        aliveNeighbourTiles++;
                    }
                }
            }
        }
        if (aliveNeighbourTiles >= m_survival) {
            return t_map[t_xPosition, t_yPosition, t_zPosition]; // Keep alive
        } else if (deadNeighbourTiles == m_birth) {
            return Random.Range(1, m_maxLives); // Birth cell
        } else {
            return t_map[t_xPosition, t_yPosition, t_zPosition] - 1;
        }
    }
}