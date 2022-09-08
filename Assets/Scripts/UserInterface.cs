using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterface : MonoBehaviour {

    #region Singleton
    public static UserInterface instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of UserInterface found!");
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] TextMeshProUGUI[] directionText;
    [SerializeField] TextMeshProUGUI actionText;
    [SerializeField] Image upButton;
    [SerializeField] Sprite[] buttonToggles;

    public void UpdateDirections(Vector2 dir, bool blocked) { 
        if (dir == new Vector2(0,1)) { //Facing North
            directionText[0].text = "W";
            directionText[1].text = "N";
            directionText[2].text = "E";
        } else if (dir == new Vector2(1,0)) { //Facing East
            directionText[0].text = "N";
            directionText[1].text = "E";
            directionText[2].text = "S";
        } else if (dir == new Vector2(0,-1)) { //Facing South
            directionText[0].text = "E";
            directionText[1].text = "S";
            directionText[2].text = "W";
        } else if (dir == new Vector2(-1,0)) { //Facing West
            directionText[0].text = "N";
            directionText[1].text = "W";
            directionText[2].text = "S";
        }
        if (blocked) {
            upButton.sprite = buttonToggles[1];
        } else {
            upButton.sprite = buttonToggles[0];
        }
    }

    public void UpdateBlurb(int actionIndex, Vector2 dir) {
        string direction = "";
        if (dir == new Vector2(0, 1)) direction = "NORTH";
        if (dir == new Vector2(1, 0)) direction = "EAST";
        if (dir == new Vector2(0, -1)) direction = "SOUTH";
        if (dir == new Vector2(-1, 0)) direction = "WEST";
        switch(actionIndex) {
            default:
                break;
            case 0:
                actionText.text = "YOU MOVE " + direction + ".";
                break;
            case 1:
                actionText.text = "YOU TURN " + direction + ".";
                break;
            case 2:
                actionText.text = "YOU ENCOUNTER A RED CIRCLE.";
                break;
            case 3:
                actionText.text = "YOU REACH YOUR DESTINATION.";
                break;
        }
    }

}
