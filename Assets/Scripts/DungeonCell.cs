using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple data class that represents each Cell of the dungeon grid
//I forget how to declare an instance of the class with itself as a function
[System.Serializable]
public class DungeonCell {

    public Vector2 coord;
    public enum CellContents { Empty, Hallway, Start, End, Red };
    public CellContents contents;

    //Should be self declaration rather than updating existing instance
    public void UpdateCell(Vector2 coordinate, CellContents content)
    {
        coord = coordinate;
        contents = content;
    }
}
