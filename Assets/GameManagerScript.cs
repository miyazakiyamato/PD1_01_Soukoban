using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //
    public GameObject playerPreFab; 
    public GameObject boxPreFab;
    int[,] map;
    GameObject[,] field;
    void PrintArray()
    {
        string debugText = "";
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y,x].ToString() + ",";
            }
            debugText += "\n";
        }
        Debug.Log(debugText);
    }
    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y,x] == null) {  continue; }
                if (field[y,x].tag == "Player")
                {
                    return new Vector2Int(x,y);
                }
            }
        }
        return new Vector2Int(-1,-1);
    }
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo,int power = 1)
    {
        if (power < 0) {  return false; }
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        //
        if (field[moveTo.y,moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;

            bool success = MoveNumber(tag, moveTo, moveTo + velocity,power - 1);
            if (!success) { return false; }
        }

        field[moveFrom.y,moveFrom.x].transform.position = new Vector3(moveTo.x - field.GetLength(1) / 2, -moveTo.y + field.GetLength(0) / 2, 0);
        field[moveTo.y,moveTo.x] = field[moveFrom.y,moveFrom.x];
        field[moveFrom.y,moveFrom.x] = null;
        return true;
    }
    bool IsCleared()
    {
        List<Vector2Int> goals = new List<Vector2Int>();
        //
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }
        //
        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y,goals[i].x];
            if (f == null || f.tag != "Box") {
                return false;
            }
        }
        Debug.Log("Clear");
        return true;
    }
    // Start is called before the first frame update
    void Start()
    {
        map = new int[,] {
            { 0,0,0,0,0},
            { 0,3,1,3,0},
            { 0,2,2,2,0},
            { 0,0,3,0,0},
            { 0,0,0,0,0},
        };
        field = new GameObject[map.GetLength(0),map.GetLength(1)];

        for(int y = 0;y < map.GetLength(0);y++)
        {
            for(int x = 0;x < map.GetLength(1); x++)
            {
                if (map[y,x] == 1)
                {
                    field[y,x] = Instantiate(
                        playerPreFab,
                        new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2 , 0),
                        Quaternion.identity
                        );
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPreFab,
                        new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 0),
                        Quaternion.identity
                        );
                }
            }
        }
        //PrintArray();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1,0));
            //PrintArray();
            IsCleared();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));
            //PrintArray();
            IsCleared();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
            //PrintArray();
            IsCleared();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));
            //PrintArray();
            IsCleared();
        }
    }
}
