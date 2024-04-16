using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    int[,] map;
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
    int GetPlayerIndexX()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0;x < map.GetLength(1); x++)
            {
                if (map[y,x] == 1)
                {
                    return x;
                }
            }
           
        }
        return -1;
    }
    int GetPlayerIndexY()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    return y;
                }
            }

        }
        return -1;
    }
    bool MoveNumberX(int number, int moveFromX, int moveToX,int moveFromY)
    {
        if (moveToX < 0 || moveToX >= map.GetLength(1)) { return false; }
        if (map[moveFromY,moveToX] == 2)
        {
            int velocity = moveToX - moveFromX;

            bool success = MoveNumberX(2, moveToX, moveToX + velocity,moveFromY);
            if (!success) { return false; }
        }

        map[moveFromY,moveToX] = number;
        map[moveFromY,moveFromX] = 0;
        return true;
    }
    // Start is called before the first frame update
    void Start()
    {
        map = new int[,] {
            { 0,0,0,0,0},
            { 0,0,1,0,0},
            { 0,0,0,0,0},
        };
        PrintArray();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int playerIndexX = GetPlayerIndexX();
            int playerIndexY = GetPlayerIndexY();
            MoveNumberX(1, playerIndexX, playerIndexX + 1,playerIndexY);
            PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int playerIndexX = GetPlayerIndexX();
            int playerIndexY = GetPlayerIndexY();
            MoveNumberX(1, playerIndexX, playerIndexX - 1, playerIndexY);
            PrintArray();
        }
    }
}
