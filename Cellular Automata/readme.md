# **Cellular Automata**

![Demo](Demo.gif)

## **Overview**
The Grid Generator is a Unity script designed to generate a grid of tiles based on specified parameters and a cellular automaton rule. It creates an evolving pattern of painted and non-painted tiles, creating interesting visual effects.

## **Features**
- **Dynamic Grid Generation**: Generate a grid of tiles with customizable height, width, and cellular automaton rule.
- **Rule-Based Tile Painting**: Each tile's state is determined by its neighbors and a predefined cellular automaton rule.
- **Randomized Starting Pattern**: Optionally start with a randomized center tile instead of a fixed center.
- **Customizable Appearance**: Set the speed of tile spawning and choose the color of painted tiles on the inspector.

## **Getting Started**

1. **Download and Open**:
    - Download the repository and open the included Unity project.
    - Open the "SampleScene" in the scenes folder.

2. **Run the Scene**:
    - Press the "Play" button in the Unity Editor to run the scene.
    - Fill the UI with the info requested and press generate.


## Script functions
- **GenerateCells**:
    - Clears the existing grid and initiates the generation of a new grid based on the specified parameters.
- **SetHeight**:
    - Sets the grid height based on the input value from a TMP_InputField.
- **SetWidth**:
    - Sets the grid width based on the input value from a TMP_InputField.
- **SetRule**:
    - Sets the cellular automaton rule based on the input value from a TMP_InputField.
- **ClearLevel**:
    - Clears all existing tiles in the grid.
- **SetUpRule**:
    - Converts the specified rule number into binary and sets up the combination array accordingly.
- **SpawnDelay**:
    - Spawns tiles in the grid with a specified delay between each tile.
- **PaintTile**:
    - Determines whether to paint the current tile based on the specified rule and the state of surrounding tiles.

## Code Example

```c#
private IEnumerator SpawnDelay() {
        int center = gridWidth / 2;
        if (randomCenterToggle.isOn) {
            center = Random.Range(0, gridWidth);
        }
        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                yield return new WaitForSeconds(speed);
                Vector3 spawnPos = new Vector3(x, -y, 0);
                GameObject tempTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                allTiles.Add(tempTile);
                tempTile.name = x.ToString() + " -" + y.ToString();
                tempTile.tag = "0"; //Not painted
                // Skip the first row
                if (y == 0) {
                    if (x == center) {
                        tempTile.GetComponent<SpriteRenderer>().material.color = paintedTileColor;
                        tempTile.tag = "1"; // Painted
                    }
                    continue;
                }
                // Start painting tiles
                Vector3 upperTiles = Vector3.zero;
                int upperTilePosY = -y + 1;
                upperTilePosY = Mathf.Abs(upperTilePosY);
                GameObject upperTileGameObject = GameObject.Find(x.ToString() + " -" + upperTilePosY.ToString());
                upperTiles.y = int.Parse(upperTileGameObject.tag.ToString());
                if (x == gridWidth - 1) {
                    upperTiles.z = 0;
                } else {
                    int upperTileLeft = x + 1; //Actually right
                    GameObject upperLeftTileGameObject = GameObject.Find(upperTileLeft.ToString() + " -" + upperTilePosY.ToString());
                    upperTiles.z = int.Parse(upperLeftTileGameObject.tag.ToString());
                }
                if (x == 0) {
                    upperTiles.x = 0;
                } else {
                    int upperTileRight = x - 1; // Actually left
                    GameObject upperRightTileGameObject = GameObject.Find(upperTileRight.ToString() + " -" + upperTilePosY.ToString());
                    upperTiles.x = int.Parse(upperRightTileGameObject.tag.ToString());
                }
                if (PaintTile(upperTiles)) {
                    tempTile.GetComponent<SpriteRenderer>().material.color = paintedTileColor;
                    tempTile.tag = "1"; // Painted
                }
            }
        }
    }
```