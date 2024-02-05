using UnityEngine;

public class CellularAutomata3D : MonoBehaviour {
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Color paintedTileColor = Color.black;

    [SerializeField] private int width = 25;
    [SerializeField] private int height = 25;
    [SerializeField] private int depth = 25;
    [SerializeField, Range(0.01f, 1f)] private float cubeProbability = 0.5f;

    private bool[,,] m_map1; //false = alive, true = dead/painted

    private void Start() {
        GenerateRandomMap();
        GenerateMapInSceneNoDelay();
    }

    private void GenerateRandomMap() {
        m_map1 = new bool[height, width, depth]; // y, x
        for (int y = 0; y < m_map1.GetLength(0); y++) {
            for (int x = 0; x < m_map1.GetLength(1); x++) {
                for (int z = 0; z < m_map1.GetLength(2); z++) {
                    if ((y == 0) || (y == height - 1) ||
                        (x == 0) || (x == width - 1) ||
                        (z == 0) || (z == depth - 1)) {
                        m_map1[y, x, z] = true;
                        continue;
                    }
                    if (Random.value >= cubeProbability) {
                        m_map1[y, x, z] = false;
                    } else {
                        m_map1[y, x, z] = true;
                    }
                }
            }
        }
    }

    private void GenerateMapInSceneNoDelay() {
        for (int y = 0; y < m_map1.GetLength(0); y++) {
            for (int x = 0; x < m_map1.GetLength(1); x++) {
                for (int z = 0; z < m_map1.GetLength(2); z++) {
                    Vector3 spawnPosition = new Vector3(x, -y, z);
                    GameObject tempTile = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
                    if (m_map1[x, y, z]) {
                        tempTile.SetActive(false);
                    } else {
                    }
                }
            }
        }
    }
}