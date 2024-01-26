using UnityEngine;

public class CellularAutomata2D : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Color paintedTileColor = Color.red;
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;
    [SerializeField, Range(0.01f, 1)] private float cubeProbability = 50f;

    private bool[,] m_map1;

    // Start is called before the first frame update
    private void Start() {
        GenerateRandomMap();
        GenerateMapInScene();
    }

    private void GenerateRandomMap() {
        m_map1 = new bool[gridHeight, gridWidth]; // y, x
        for (int y = 0; y < m_map1.GetLength(0); y++) {
            for (int x = 0; x < m_map1.GetLength(1); x++) {
                if ((y == 0) || (y == gridHeight - 1) ||
                    (x == 0) || (x == gridWidth - 1)) {
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

    private void GenerateMapInScene() {
        for (int y = 0; y < m_map1.GetLength(0); y++) {
            for (int x = 0; x < m_map1.GetLength(1); x++) {
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
    }
}