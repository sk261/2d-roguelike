using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum, maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8, rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit, wallTiles, outerWallTiles, floorTiles;
    public GameObject[] foodTiles, enemyTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        
        for (int x = -1; x < columns + 1; x++)
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toCopy = floorTiles;
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toCopy = outerWallTiles;
                MultiSpriteHolder sprites = toCopy.GetComponent<MultiSpriteHolder>();
                sprites.SetSprite(Random.Range(0, sprites.length()));
                GameObject newObj = Instantiate(toCopy, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                newObj.name = "(" + x + "," + y + ") Tile";
                newObj.transform.SetParent(boardHolder);
            }
    }

    Vector3 RandomPosition()
    {
        int index = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[index];
        gridPositions.RemoveAt(index);
        return pos;
    }

    void LayoutObjects(object tiles, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 pos = RandomPosition();
            GameObject tile;
            if (tiles.GetType().IsArray)
                tile = ((tiles as object[])[Random.Range(0, (tiles as object[]).Length)]) as GameObject;
            else
            {
                tile = tiles as GameObject;
                MultiSpriteHolder sprites = tile.GetComponent<MultiSpriteHolder>();
                sprites.SetSprite(Random.Range(0, sprites.length()));
            }
            GameObject obj = Instantiate(tile, pos, Quaternion.identity);
            obj.name = "(" + pos.x + ", " + pos.y + ") " + obj.name.Remove(obj.name.Length - 7);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitList();
        LayoutObjects(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjects(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjects(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
