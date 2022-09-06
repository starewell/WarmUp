using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour {

    DungeonLayout layout;
    [SerializeField]
    CellVisuals[] visuals;

    public Vector2 pos;
    public Vector2 direction;

    void Start() {
        if (DungeonLayout.instance != null) layout = DungeonLayout.instance;

        Initialize();
    }

    void Initialize() {
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

    void CycleVisuals() {
        int i = 1;
        bool[] toggles = { false, false, false, false};
        foreach(CellVisuals vis in visuals) {
            DungeonCell nextCell = layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x * i, pos.y + direction.y * i));
            if (nextCell.contents != DungeonCell.CellContents.Empty) { // if there is something to be drawn
                if (direction.x != 0) { //Check north south 
                    DungeonCell adjCell = layout.cells.Find(cell => cell.coord == new Vector2(nextCell.coord.x, nextCell.coord.y + direction.x));
                    if (adjCell.contents != DungeonCell.CellContents.Empty) {
                        toggles[1] = true;
                    }
                    adjCell = layout.cells.Find(cell => cell.coord == new Vector2(nextCell.coord.x, nextCell.coord.y - direction.x));
                    if (adjCell.contents != DungeonCell.CellContents.Empty) {
                        toggles[2] = true;
                    }
                } else { //Check east west 
                    DungeonCell adjCell = layout.cells.Find(cell => cell.coord == new Vector2(nextCell.coord.x + direction.y, nextCell.coord.y));
                    if (adjCell.contents != DungeonCell.CellContents.Empty) {
                        toggles[1] = true;
                    }
                    adjCell = layout.cells.Find(cell => cell.coord == new Vector2(nextCell.coord.x - direction.y, nextCell.coord.y));
                    if (adjCell.contents != DungeonCell.CellContents.Empty) {
                        toggles[2] = true;
                    }
                }
                nextCell = layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x * i, pos.y + direction.y * i));
                if (nextCell.contents != DungeonCell.CellContents.Empty) {
                    vis.UpdateVisuals();
                }
            } else vis.UpdateVisuals(false, false, false, false);
            i++;
        }
    }
}
