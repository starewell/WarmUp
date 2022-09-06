using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DungeonLayout : MonoBehaviour {

    #region Singleton
    public static DungeonLayout instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of DungeonLayout found!");
            return;
        }
        instance = this;
    }
    #endregion

    const string FILE_NAME = "DungeonLayout.txt";

    [SerializeField]
    public List<DungeonCell> cells = new List<DungeonCell>();

    void Start() {
        ReadDungeonFromFile();
    }

    public void ReadDungeonFromFile()
    {
        StreamReader reader = new StreamReader(FILE_NAME);
        string fileContent = reader.ReadToEnd();
        reader.Close();

        char[] newLineChar = { '\n' };
        string[] fileLine = fileContent.Split(newLineChar);

        int x = 0; int y = 0;

        foreach(string line in fileLine) { 
            foreach(char cell in line) {
                switch(cell) {
                    default:
                        CreateNewCell(x, y, DungeonCell.CellContents.Empty);
                        break;
                    case '\n':
                        break;
                    case 'X':
                        CreateNewCell(x, y, DungeonCell.CellContents.Empty);
                        break;
                    case 'O':
                        CreateNewCell(x ,y , DungeonCell.CellContents.Hallway);
                        break;
                    case 'S':
                        CreateNewCell(x, y, DungeonCell.CellContents.Start);
                        break;
                }
                Debug.Log(cell);
                x++;
            }
            x = 0;
            y++;
        }
    }

    void CreateNewCell(int x, int y, DungeonCell.CellContents type) {
        Debug.Log(x + ", " + y);
        DungeonCell newCell = new DungeonCell();
        newCell.UpdateCell(new Vector2(x, y), type);
        cells.Add(newCell);
    }
}
