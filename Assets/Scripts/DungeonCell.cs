using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonCell 
{

    public Vector2 coord;
    public enum CellContents { Empty, Hallway, Start, End, Red };
    public CellContents contents;

    public void UpdateCell(Vector2 coordinate, CellContents content)
    {
        coord = coordinate;
        contents = content;
    }


}
