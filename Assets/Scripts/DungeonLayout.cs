using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Script responsible for initializing the dungeon grid, read from file
public class DungeonLayout : MonoBehaviour {

    //Also includes first call to ReadDungeonCell bc order of operations... should fix
    #region Singleton
    public static DungeonLayout instance;
    void Awake() {
        if (instance != null)
        {
            Debug.Log("More than one instance of DungeonLayout found!");
            return;
        }
        instance = this;

        ReadDungeonFromFile();
    }
    #endregion

    //Level editor in txt file
    const string FILE_NAME = "DungeonLayout.txt";

    [SerializeField]
    public List<DungeonCell> cells = new List<DungeonCell>();

    //Read file and create new list of Dungeon Cells with contents assigned from txt file
    public void ReadDungeonFromFile() {
        StreamReader reader = new StreamReader(FILE_NAME);
        string fileContent = reader.ReadToEnd();
        reader.Close();

        char[] newLineChar = { '\n' };
        string[] fileLine = fileContent.Split(newLineChar);

        int x = 0; int y = 0;

        //Loop through each character of the file and assign contents and coordinate to a cell
        foreach(string line in fileLine) { 
            foreach(char cell in line) {
                switch(cell) {
                    default:
                        CreateNewCell(x, y, DungeonCell.CellContents.Empty);
                        break;
                    case '\n':
                        break;
                    case ' ':
                        CreateNewCell(x, y, DungeonCell.CellContents.Empty);
                        break;
                    case 'O':
                        CreateNewCell(x ,y , DungeonCell.CellContents.Hallway);
                        break;
                    case 'S':
                        CreateNewCell(x, y, DungeonCell.CellContents.Start);
                        break;
                    case 'E':
                        CreateNewCell(x, y, DungeonCell.CellContents.End);
                        break;
                    case 'R':
                        CreateNewCell(x, y, DungeonCell.CellContents.Red);
                        break;
                }
                x++;
            }
            x = 0;
            y++;
        }
    }

    //Function to create cell and update values
    void CreateNewCell(int x, int y, DungeonCell.CellContents type) {
        DungeonCell newCell = new DungeonCell();
        newCell.UpdateCell(new Vector2(x, y), type);
        cells.Add(newCell);
    }
}
