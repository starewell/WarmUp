using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVisuals : MonoBehaviour {

    public GameObject rightWall, leftWall, backWall;
    public List<GameObject> rightHallway, leftHallway;

    public void UpdateVisuals(bool visible, bool openLeft, bool openRight, bool closed) { 
        if (visible) { 
            if (openLeft) {
                leftWall.SetActive(false);
                foreach(GameObject go in leftHallway) go.SetActive(true);             
            } else { 
                leftWall.SetActive(true);
                foreach(GameObject go in leftHallway) go.SetActive(false);               
            }
            if (openRight) {
                rightWall.SetActive(false);
                foreach(GameObject go in rightHallway) go.SetActive(true);                
            } else { 
                rightWall.SetActive(true);
                foreach(GameObject go in rightHallway) go.SetActive(false);           
            }
        } else {
            rightWall.SetActive(false); leftWall.SetActive(false); backWall.SetActive(false);
            foreach (GameObject go in leftHallway) go.SetActive(false);
            foreach (GameObject go in rightHallway) go.SetActive(false);
        }
    }

}
