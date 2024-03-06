using UnityEngine;

public class TileController : MonoBehaviour {
    [SerializeField] private Color grassColor = Color.green;
    [SerializeField] private Color earthColor = Color.yellow;
    [SerializeField] private Color waterColor = Color.blue;
    [SerializeField] private GameObject treePrefab;
    [SerializeField, Range(0, 1)] private float treeProbability = 0.5f;

    private TileType tileType;
    private int earthTilesAroundTile;

    // Start is called before the first frame update
    private void Start() {
    }

    public void setTileType(TileType t_tileType) {
        tileType = t_tileType;
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
        }
    }

    private void setUpEarth() {
        setColor(earthColor);
        treePrefab.SetActive(false);
    }

    private void setUpGrass() {
        setColor(grassColor);
        treePrefab.SetActive(Random.value < treeProbability);
    }

    private void setUpWater() {
        setColor(waterColor);
        treePrefab.SetActive(false);
    }

    private void setColor(Color t_color) {
        GetComponent<MeshRenderer>().material.color = t_color;
    }
}

public enum TileType {
    None,
    Grass,
    Earth,
    Water
}