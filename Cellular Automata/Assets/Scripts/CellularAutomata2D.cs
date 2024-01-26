using UnityEngine;

public class CellularAutomata2D : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Color paintedTileColor = Color.red;
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;
    [SerializeField, Range(1, 9)] private int neighbourCellsToBeAlive = 3;
    [SerializeField, Range(1, 9)] private int neighbourCellsToBeDead = 4;
    [SerializeField,] private int iterations = 2;
    [SerializeField, Range(0.01f, 1)] private float cubeProbability = 50f;

    private bool[,] m_map1;

    // Start is called before the first frame update
    private void Start() {
        GenerateRandomMap();
        IterateNewMap();
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

    private void IterateNewMap() {
        for (int i = 0; i < iterations; i++) {
            bool[,] map2 = m_map1;
            for (int y = 0; y < map2.GetLength(0); y++) {
                for (int x = 0; x < map2.GetLength(1); x++) {
                    if ((y == 0) || (y == gridHeight - 1) ||
                        (x == 0) || (x == gridWidth - 1)) {
                        map2[y, x] = true;
                        continue;
                    }
                    map2[y, x] = Checktiles(map2, x, y);
                }
            }
            m_map1 = map2;
        }
    }

    private bool Checktiles(bool[,] t_map, int t_xPosition, int t_yPosition) {
        int aliveTiles = 0, deadTiles = 0;
        for (int y = t_yPosition - 1; y <= t_yPosition + 1; y++) {
            for (int x = t_xPosition - 1; x <= t_xPosition + 1; x++) {
                if (y == t_yPosition || x == t_xPosition) {
                    continue;
                }
                if (t_map[y, x]) {
                    aliveTiles++;
                } else {
                    deadTiles++;
                }
            }
        }
        if (aliveTiles == neighbourCellsToBeAlive) {
            return true;
        } else if (deadTiles == neighbourCellsToBeDead) {
            return false;
        } else {
            return t_map[t_yPosition, t_xPosition];
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