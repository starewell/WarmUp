using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for tracking the players position in the dungeon grid and communicating w/ UI
//Also sloppily tracks and updates certain visual elements
//This class should broadcast an event when this happens instead, and the visual tracking should be refactored into a new class
[RequireComponent(typeof(AudioSource))]
public class DungeonCrawler : MonoBehaviour {

    DungeonLayout layout;
    UserInterface ui;

    [SerializeField]
    CellVisuals[] visuals;
    [SerializeField]
    GameObject deadEnd;
    AudioSource audioSource;
    [SerializeField]
    AudioClip walkSFX, winSFX, bumpSFX;

    public Vector2 pos;
    public Vector2 direction;

    void Start() {
        //Get refs
        if (DungeonLayout.instance != null) layout = DungeonLayout.instance;
        if (UserInterface.instance != null) ui = UserInterface.instance;
        audioSource = GetComponent<AudioSource>();

        //Initialize
        pos = layout.cells.Find(cell => cell.contents == DungeonCell.CellContents.Start).coord;
        direction = new Vector2(0, 1);

        StartCoroutine(CycleVisuals());
    }

    //Lazy input checks for movement
    void Update()
    {
        //Turn left
        if (Input.GetKeyDown(KeyCode.A)) {
            Movement(-1);
        }
        //Turn right
        if (Input.GetKeyDown(KeyCode.D)) {
            Movement(1);
        }
        //Move forward
        if (Input.GetKeyDown(KeyCode.W)) {
            Movement(0);
           
        }
    }

    public void Movement(int dir) { 
        if (dir == -1) {
            //Change direction, update visuals and UI
            if (direction.x == 1) direction = new Vector2(0, 1);
            else if (direction.x == -1) direction = new Vector2(0, -1);
            else if (direction.y == 1) direction = new Vector2(-1, 0);
            else if (direction.y == -1) direction = new Vector2(1, 0);
            StartCoroutine(CycleVisuals());
            ui.UpdateBlurb(1, direction);
        } if (dir == 1) {
            //Change direction, update visuals and UI
            if (direction.x == 1) direction = new Vector2(0, -1);
            else if (direction.x == -1) direction = new Vector2(0, 1);
            else if (direction.y == 1) direction = new Vector2(1, 0);
            else if (direction.y == -1) direction = new Vector2(-1, 0);
            StartCoroutine(CycleVisuals());
            ui.UpdateBlurb(1, direction);
        }
        if (dir == 0) {
            DungeonCell nextCell = layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x, pos.y + direction.y));
            //Check if player can walk forward
            if (nextCell.contents != DungeonCell.CellContents.Empty) {
                //Update position and cell visuals
                pos += direction;
                StartCoroutine(CycleVisuals());
                //Check if the cell has an object, update UI and play appropriate SFX
                if (nextCell.contents == DungeonCell.CellContents.Red) {
                    ui.UpdateBlurb(2, direction);
                    audioSource.PlayOneShot(bumpSFX);
                }
                else if (nextCell.contents == DungeonCell.CellContents.End) {
                    ui.UpdateBlurb(3, direction);
                    audioSource.PlayOneShot(winSFX);
                } else {
                    ui.UpdateBlurb(0, direction);
                    audioSource.PlayOneShot(walkSFX);
                }
            }
            
        }
    }

    IEnumerator CycleVisuals() {
        //Loop index and visual cutoff bool
        int i = 1;
        bool blockedView = false;
        ui.UpdateDirections(direction, false);
        //Loop through the serialized list of slices and update toggles for what's visible in each
        foreach (CellVisuals vis in visuals) {
            //...What a weird choice. Bool array for hidden toggles, detailed in CellVisuals 
            bool[] toggles = { false, false, false, false, false };
            int colorIndex = 0; //What color the object is

            //If this cell isn't already blocked 
            if (!blockedView) { 
                //Reference the next cell forward from the player
                DungeonCell nextCell = layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x * i, pos.y + direction.y * i));
                if (nextCell != null && nextCell.contents != DungeonCell.CellContents.Empty) { // if there is something to be drawn                 
                    if (i == 1) deadEnd.SetActive(false); //Override
                    //Check if there is an object in this cell
                    if (nextCell != null && nextCell.contents == DungeonCell.CellContents.End) {
                        toggles[4] = true;
                        colorIndex = 0;
                    } else if (nextCell != null && nextCell.contents == DungeonCell.CellContents.Red) {
                        toggles[4] = true;
                        colorIndex = 1;
                    }
                    //Check for open hallways in adjacent cells
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
                    //Check if this cell is closed at the end
                    nextCell = layout.cells.Find(cell => cell.coord == new Vector2(pos.x + direction.x * i, pos.y + direction.y * i) + direction);
                    if (nextCell.contents == DungeonCell.CellContents.Empty) { // check if closed
                        toggles[3] = true;
                    }

                } else {
                    blockedView = true;
                    if (i == 1) {
                        deadEnd.SetActive(true);
                        ui.UpdateDirections(direction, true);
                    }
                    toggles[0] = true;
                }
            } else toggles[0] = true;
            //Pass recorded toggles and iterate
            vis.UpdateVisuals(toggles, colorIndex);
            i++;
            yield return new WaitForSeconds(.05f);
        }
    }
}
