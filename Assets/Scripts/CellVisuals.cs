using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for updating the sprites for each visible row of the dungeon grid
public class CellVisuals : MonoBehaviour {

    //Visual references
    public GameObject slice, rightWall, leftWall, backWall, obj;
    public List<GameObject> rightHallway, leftHallway;
    [SerializeField]
    Sprite[] objColors;
    int colorIndex;

    //Toggles for what should be made visible
    //hidden, openLeft, openRight, closed, object
    bool[] hidden = { false, false, false, false, true };

    //Store paramaters locally. Wouldn't need to if I could pass paramaters through public coroutines
    public void UpdateVisuals(bool[] paramaters, int cI) {
        hidden = paramaters;
        colorIndex = cI;
        StartCoroutine(FakeScan());
    }

    //Coroutine which animates the visual updates, makes a fake scan
    IEnumerator FakeScan() {
        //Hide everything
        slice.SetActive(false);
        yield return new WaitForSeconds(.05f); //Delay to draw new slice

        if (!hidden[0]) { //If there's something to draw
            slice.SetActive(true);

            if (hidden[1]) { //Toggles Left Hallway
                leftWall.SetActive(false);
                foreach(GameObject go in leftHallway) go.SetActive(true);             
            } else { 
                leftWall.SetActive(true);
                foreach(GameObject go in leftHallway) go.SetActive(false);               
            }

            if (hidden[2]) { //Toggles Right Hallway
                rightWall.SetActive(false);
                foreach(GameObject go in rightHallway) go.SetActive(true);                
            } else { 
                rightWall.SetActive(true);
                foreach(GameObject go in rightHallway) go.SetActive(false);           
            }

            if (hidden[3]) { //Toggles Back Wall
                backWall.SetActive(true);
            } else {
                backWall.SetActive(false);
            }

            if (hidden[4]) { //Toggles object displayed
                obj.SetActive(true);
                obj.GetComponent<SpriteRenderer>().sprite = objColors[colorIndex];
            } else obj.SetActive(false);

        } else { //There's nothing to draw
            slice.SetActive(false);
        }
    }

}
