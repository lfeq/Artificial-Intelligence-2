using Unity.VisualScripting;
using UnityEngine;

public class TileController : MonoBehaviour {
    [SerializeField, Header("Tile Colors")] private Color grassColor = Color.green;
    [SerializeField] private Color earthColor = Color.yellow;
    [SerializeField] private Color waterColor = Color.blue;
    [SerializeField, Header("Tree")] private GameObject treePrefab;
    [SerializeField, Range(0, 1)] private float treeProbability = 0.5f;
    [SerializeField, Header("Bush")] private GameObject bushPrefab;
    [SerializeField, Range(0, 1)] private float bushProbability = 0.5f;
    [SerializeField] private float bushSpawnTimeInSeconds = 30;
    [SerializeField, Header("Rock")] private GameObject rockPrefab;

    private TileType tileType;
    private int earthTilesAroundTile;
    private float bushSpawnCountdown;

    private void Start() {
        bushSpawnCountdown = bushSpawnTimeInSeconds;
    }

    private void Update() {
        if (tileType != TileType.Grass) {
            return;
        }
        growBush();
    }

    public void setTileType(TileType t_tileType) {
        tileType = t_tileType;
    }

    public TileType getTileType() {
        return tileType;
    }

    public void setEarthTileAroundTile(int t_earthTilesAroundTile) {
        earthTilesAroundTile = t_earthTilesAroundTile;
    }

    public void setupTiles() {
        switch (tileType) {
            case TileType.Earth:
                setUpEarth();
                break;
            case TileType.Grass:
                setUpGrass();
                break;
            case TileType.Water:
                setUpWater();
                break;
            case TileType.Border:
                setUpBorder();
                break;
        }
    }

    private void setUpBorder() {
        if (earthTilesAroundTile != 9) {
            setColor(earthColor);
        } else {
            setColor(grassColor);
        }
        setTree(false);
        setBush(false);
        setRock(true);
    }

    private void setUpEarth() {
        setColor(earthColor);
        setTree(false);
        setBush(false);
        setRock(false);
    }

    private void setUpGrass() {
        setColor(grassColor);
        setTree(Random.value < treeProbability);
        setBush(Random.value < bushProbability);
        setRock(false);
    }

    private void setUpWater() {
        setColor(waterColor);
        setTree(false);
        setBush(false);
        setRock(false);
        gameObject.tag = "Obstacle";
    }

    private void setColor(Color t_color) {
        GetComponent<MeshRenderer>().material.color = t_color;
    }

    private void setTree(bool t_isTreeActive) {
        treePrefab.SetActive(t_isTreeActive);
    }

    private void setBush(bool t_isBushActive) {
        if (treePrefab.activeSelf) {
            return;
        }
        bushPrefab.SetActive(t_isBushActive);
    }

    private void setRock(bool t_isRockActive) {
        rockPrefab.SetActive(t_isRockActive);
    }

    private void growBush() {
        if (bushPrefab.activeSelf || treePrefab.activeSelf) {
            return;
        }
        bushSpawnCountdown -= Time.deltaTime;
        if (bushSpawnCountdown < 0) {
            setBush(Random.value < bushProbability);
            bushSpawnCountdown = bushSpawnTimeInSeconds;
        } // Spawn bush
    }
}

public enum TileType {
    None,
    Grass,
    Earth,
    Water,
    Border
}