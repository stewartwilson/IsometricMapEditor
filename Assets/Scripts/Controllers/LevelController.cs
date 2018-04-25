using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class LevelController : MonoBehaviour {
    public LevelData levelData;
    public bool generateMapObjects;
    public bool updateLevelData;
    public bool updateLevelDataBasic;


    // Use this for initialization
    void Awake() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (updateLevelData)
        {
            levelData.generateMapFromTextFile();
        }
        else if (updateLevelDataBasic)
        {
            levelData.generateBasicFlatMap(10, 10);
        }
        if (generateMapObjects)
        {
            InstantiateLevelMap();
        }
    }

    private void InstantiateLevelMap()
    {
        int count = 0;
        foreach(MapTile mt in levelData.map)
        {
            count++;
            GameObject go = (GameObject)Instantiate(Resources.Load("Tile"));
            go.transform.SetParent(GameObject.Find("Tiles").transform);
            go.transform.position = IsometricHelper.gridToGamePostion(mt.position);
            go.GetComponent<SpriteRenderer>().sortingOrder = IsometricHelper.getTileSortingOrder(mt.position);
            go.name = "Tile " + count;
            
        }
    }

    private void ExportLevelMap()
    {
        int count = 0;
        foreach (MapTile mt in levelData.map)
        {
            count++;
            GameObject go = (GameObject)Instantiate(Resources.Load("Grass Tile"));
            go.transform.SetParent(GameObject.Find("Tiles").transform);
            go.transform.position = IsometricHelper.gridToGamePostion(mt.position);
            go.GetComponent<SpriteRenderer>().sortingOrder = IsometricHelper.getTileSortingOrder(mt.position);
            go.name = "Tile " + count;

        }
    }

}
