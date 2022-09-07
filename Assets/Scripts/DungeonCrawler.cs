using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour {

    DungeonLayout layout;
    [SerializeField]
    CellVisuals[] visuals;
    [SerializeField]
    GameObject deadEnd;

    public Vector2 pos;
    public Vector2 direction;

    void Start() {
        if (DungeonLayout.instance != null) layout = DungeonLayout.instance;

        pos = layout.cells.Find(cell => cell.contents == DungeonCell.CellContents.Start).coord;
        direction = new Vector2(0, 1);

        StartCoroutine(CycleVisuals());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) { 
            if (direction.x == 1) direction = new Vector2(0, 1);
            else if (direction.x == -1) direction = new Vector2(0, -1);
            else if (direction.y == 1) direction = new Vector2(-1, 0);
            else if (direction.y == -1) direction = new Vector2(1, 0);
            StartCoroutine(CycleVisuals());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (direction.x == 1) direction = new Vector2(0, -1);
            else if (direction.x == -1) direction = new Vector2(0, 1);
            else if (direction.y == 1) direction = new Vector2(1, 0);
            else if (direction.y == -1) direction = new Vector2(-1, 0);
            StartCoroutine(CycleVisuals());
        }
        if (Input.GetKeyDown(KeyCode.W)) { 
            if (layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x, pos.y + direction.y)).contents != DungeonCell.CellContents.Empty) {
                pos += direction;
                StartCoroutine(CycleVisuals());
            }
        }
    }

    IEnumerator WaitForInput() {
        yield return null;
    }

    IEnumerator CycleVisuals() {
        Debug.Log(layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x, pos.y + direction.y)).contents);
        int i = 1;
        bool blockedView = false;
        foreach (CellVisuals vis in visuals) {
            bool[] toggles = { false, false, false, false };
            if (!blockedView) { 
                DungeonCell nextCell = layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x * i, pos.y + direction.y * i));
                if (nextCell != null && nextCell.contents != DungeonCell.CellContents.Empty) { // if there is something to be drawn
                    if (i == 1) deadEnd.SetActive(false);
                    if (direction.x != 0) { //Check north south 
                        DungeonCell adjCell = layout.cells.Find(cell => cell.coord == new Vector2(nextCell.coord.x, nextCell.coord.y + direction.x));
                        if (adjCell.contents != DungeonCell.CellContents.Empty) {
                            toggles[1] = true;
                        }
                        adjCell = layout.cells.Find(cell => cell.coord == new Vector2(nextCell.coord.x, nextCell.coord.y - direction.x));
                        if (adjCell.contents != DungeonCell.CellContents.Empty) {
                            toggles[2] = true;
                        }
                    } else if (direction.y != 0) { //Check east west 
                        DungeonCell adjCell = layout.cells.Find(cell => cell.coord == new Vector2(nextCell.coord.x - direction.y, nextCell.coord.y));
                        if (adjCell.contents != DungeonCell.CellContents.Empty) {
                            toggles[1] = true;
                        }
                        adjCell = layout.cells.Find(cell => cell.coord == new Vector2(nextCell.coord.x + direction.y, nextCell.coord.y));
                        if (adjCell.contents != DungeonCell.CellContents.Empty) {
                            toggles[2] = true;
                        }
                    }
                    nextCell = layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x * i, pos.y + direction.y * i) + direction);
                    if (nextCell.contents == DungeonCell.CellContents.Empty) { // check if closed
                        toggles[3] = true;
                    }
                } else {
                    blockedView = true;
                    if (i == 1) deadEnd.SetActive(true);
                    toggles[0] = true;
                }
            } else toggles[0] = true;
            vis.UpdateVisuals(toggles);
            i++;
            yield return new WaitForSeconds(.025f);
        }
        
    }
}
