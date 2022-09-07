using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVisuals : MonoBehaviour {

    [SerializeField]
    bool slice00;
    public GameObject slice, rightWall, leftWall, backWall;
    public List<GameObject> rightHallway, leftHallway;

    //hidden, openLeft, openRight, closed
    bool[] hidden = { false, false, false, false };

    public void UpdateVisuals(bool[] paramaters) {
        hidden = paramaters;
        StartCoroutine(FakeScan());
    }

    IEnumerator FakeScan() {
        slice.SetActive(false);
        yield return new WaitForSeconds(.025f);
        if (!hidden[0]) {
                slice.SetActive(true);
                if (!slice00) { 
                    if (hidden[1]) {
                        leftWall.SetActive(false);
                        foreach(GameObject go in leftHallway) go.SetActive(true);             
                    } else { 
                        leftWall.SetActive(true);
                        foreach(GameObject go in leftHallway) go.SetActive(false);               
                    }
                    if (hidden[2]) {
                        rightWall.SetActive(false);
                        foreach(GameObject go in rightHallway) go.SetActive(true);                
                    } else { 
                        rightWall.SetActive(true);
                        foreach(GameObject go in rightHallway) go.SetActive(false);           
                    }
                    if (hidden[3]) {
                        backWall.SetActive(true);
                    } else {
                        backWall.SetActive(false);
                    }
                } else {
                    if (hidden[3]) { backWall.SetActive(true); }
                    else backWall.SetActive(false);
                }
            } else {
                backWall.SetActive(false);
                slice.SetActive(false);
            }
    }

}
