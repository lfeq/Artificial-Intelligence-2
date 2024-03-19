using UnityEngine;

/// <summary>
/// Controls the properties and behavior of individual tiles in the cellular automata map.
/// </summary>
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

    /// <summary>
    /// Sets the type of the tile.
    /// </summary>
    /// <param name="t_tileType">The type of the tile.</param>
    public void setTileType(TileType t_tileType) {
        tileType = t_tileType;
    }

    /// <summary>
    /// Gets the type of the tile.
    /// </summary>
    /// <returns>The type of the tile.</returns>
    public TileType getTileType() {
        return tileType;
    }

    /// <summary>
    /// Sets the number of earth tiles around the current tile.
    /// </summary>
    /// <param name="t_earthTilesAroundTile">The number of earth tiles around the current tile.</param>
    public void setEarthTileAroundTile(int t_earthTilesAroundTile) {
        earthTilesAroundTile = t_earthTilesAroundTile;
    }

    /// <summary>
    /// Sets up the properties of the tile based on its type.
    /// </summary>
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

    /// <summary>
    /// Makes the tile as border tile.
    /// </summary>
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

    /// <summary>
    /// Makes the tile as earth tile.
    /// </summary>
    private void setUpEarth() {
        setColor(earthColor);
        setTree(false);
        setBush(false);
        setRock(false);
    }

    /// <summary>
    /// Makes the tile as grass tile.
    /// </summary>
    private void setUpGrass() {
        setColor(grassColor);
        setTree(Random.value < treeProbability);
        setBush(Random.value < bushProbability);
        setRock(false);
    }

    /// <summary>
    /// Makes the tile as water tile.
    /// </summary>
    private void setUpWater() {
        setColor(waterColor);
        setTree(false);
        setBush(false);
        setRock(false);
        gameObject.tag = "Obstacle";
    }

    /// <summary>
    /// Sets the tile color.
    /// </summary>
    private void setColor(Color t_color) {
        GetComponent<MeshRenderer>().material.color = t_color;
    }

    /// <summary>
    /// Sets the tree status for the tile.
    /// </summary>
    private void setTree(bool t_isTreeActive) {
        treePrefab.SetActive(t_isTreeActive);
    }

    /// <summary>
    /// Sets the bush status for the tile.
    /// </summary>
    private void setBush(bool t_isBushActive) {
        if (treePrefab.activeSelf) {
            return;
        }
        bushPrefab.SetActive(t_isBushActive);
    }

    /// <summary>
    /// Sets the rock status for the tile.
    /// </summary>
    private void setRock(bool t_isRockActive) {
        rockPrefab.SetActive(t_isRockActive);
    }

    /// <summary>
    /// Grows a bush on the grass tile over time.
    /// </summary>
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

/// <summary>
/// Enumeration of tile types.
/// </summary>
public enum TileType {
    None,
    Grass,
    Earth,
    Water,
    Border
}