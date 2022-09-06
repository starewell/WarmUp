using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour {

    DungeonLayout layout;

    public Vector2 pos;
    public Vector2 direction;

    void Start() {
        if (DungeonLayout.instance != null) layout = DungeonLayout.instance;

        StartCoroutine(Initialize());
    }

    IEnumerator Initialize() {
        yield return new WaitForSeconds(.25f);
        foreach(DungeonCell cell in layout.cells) {

            if (cell.contents == DungeonCell.CellContents.Start) {
                pos = new Vector2(cell.coord.x, cell.coord.y);
            }
        }
        direction = new Vector2(0, -1);
    }

    IEnumerator WaitForInput() {
        yield return null;
    }

    void CheckValidMovement() {
        DungeonCell nextCell = layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x, pos.y + direction.y));

        if (nextCell.contents != DungeonCell.CellContents.Empty) {

        }
        
    }
}
