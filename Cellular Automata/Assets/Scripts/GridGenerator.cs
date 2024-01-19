using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Generates a grid of tiles based on specified parameters and a cellular automaton rule.
/// </summary>
public class GridGenerator : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private Color paintedTileColor = Color.red;
    [SerializeField] private TMP_InputField heightInput;
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField ruleInput;
    [SerializeField] private Toggle randomCenterToggle;

    private int gridWidth = 250;
    private int gridHeight = 250;
    private int rule = 250;
    private int[] combination = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
    private List<GameObject> allTiles = new List<GameObject>();

    /// <summary>
    /// Generates the grid of tiles based on the specified parameters and cellular automaton rule.
    /// </summary>
    public void GenerateCells() {
        ClearLevel();
        SetUpRule();
        StartCoroutine(SpawnDelay());
    }

    /// <summary>
    /// Sets the grid height based on the input field value.
    /// </summary>
    public void SetHeight() {
        gridHeight = int.Parse(heightInput.text);
    }

    /// <summary>
    /// Sets the grid width based on the input field value.
    /// </summary>
    public void SetWidth() {
        gridWidth = int.Parse(widthInput.text);
    }

    /// <summary>
    /// Sets the cellular automaton rule based on the input field value.
    /// </summary>
    public void SetRule() {
        rule = int.Parse(ruleInput.text);
    }

    /// <summary>
    /// Clears all existing tiles in the grid.
    /// </summary>
    private void ClearLevel() {
        for (int i = 0; i < allTiles.Count; i++) {
            Destroy(allTiles[i]);
        }
        allTiles.Clear();
    }

    /// <summary>
    /// Sets up the cellular automaton rule based on the specified rule number.
    /// </summary>
    // Convert number to binary code got from this link:
    // https://stackoverflow.com/questions/2954962/convert-integer-to-binary-in-c-sharp
    // Convert char to int code got from this link:
    // https://stackoverflow.com/questions/239103/convert-char-to-int-in-c-sharp
    private void SetUpRule() {
        if (rule > 255) {
            throw new Exception("Rule number must be smaller than 256.");
        }
        string binary = Convert.ToString(rule, 2);
        int j = 1;
        for (int i = combination.Length - 1; i >= 0 - 1; i--, j++) {
            if (j > binary.Length) {
                break;
            }
            combination[i] = binary[binary.Length - j] - '0';
        }
    }

    /// <summary>
    /// Spawns tiles in the grid with a specified delay between each tile.
    /// </summary>
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

    /// <summary>
    /// Determines whether to paint the current tile based on the specified rule and the state of surrounding tiles.
    /// </summary>
    /// <param name="upperTreeTiles">Vector3 representing the tiles upwards to the current tile, where x is the one up to the left, y the one above, and z the one up to the right.</param>
    /// <returns>
    /// <c>true</c> if the current tile should be painted according to the cellular automaton rule;
    /// <c>false</c> if the current tile should not be painted.
    /// </returns>
    private bool PaintTile(Vector3 upperTreeTiles) {
        if ((upperTreeTiles.x == 1) && (upperTreeTiles.y == 1) && (upperTreeTiles.z == 1)) {
            return Convert.ToBoolean(combination[0]);
        } else if ((upperTreeTiles.x == 1) && (upperTreeTiles.y == 1) && (upperTreeTiles.z == 0)) {
            return Convert.ToBoolean(combination[1]);
        } else if ((upperTreeTiles.x == 1) && (upperTreeTiles.y == 0) && (upperTreeTiles.z == 1)) {
            return Convert.ToBoolean(combination[2]);
        } else if ((upperTreeTiles.x == 1) && (upperTreeTiles.y == 0) && (upperTreeTiles.z == 0)) {
            return Convert.ToBoolean(combination[3]);
        } else if ((upperTreeTiles.x == 0) && (upperTreeTiles.y == 1) && (upperTreeTiles.z == 1)) {
            return Convert.ToBoolean(combination[4]);
        } else if ((upperTreeTiles.x == 0) && (upperTreeTiles.y == 1) && (upperTreeTiles.z == 0)) {
            return Convert.ToBoolean(combination[5]);
        } else if ((upperTreeTiles.x == 0) && (upperTreeTiles.y == 0) && (upperTreeTiles.z == 1)) {
            return Convert.ToBoolean(combination[6]);
        } else if ((upperTreeTiles.x == 0) && (upperTreeTiles.y == 0) && (upperTreeTiles.z == 0)) {
            return Convert.ToBoolean(combination[7]);
        } else {
            throw new Exception("Something went terrebly wrong :(");
        }
    }
}