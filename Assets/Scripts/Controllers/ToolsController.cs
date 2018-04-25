using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsController : MonoBehaviour
{

    public Slider widthSlider;
    public Text widthValue;
    public int currentWidth = 1;
    public Slider heightSlider;
    public Text heightValue;
    public int currentHeight = 1;
    public Slider elevationSlider;
    public Text elevationValue;
    public Dropdown spriteType;
    public Image spriteImage;
    public List<GameObject> selectedTiles;
    public InputField levelName;
    public Dropdown savedLevels;

    private bool tileSelected = false;


    // Use this for initialization
    void Start()
    {
        createBasicTile(0, 0);
        getSavedLevels();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Clear"))
        {
            selectedTiles.Clear();
            tileSelected = false;
        }
        widthValue.text = widthSlider.value.ToString();
        heightValue.text = heightSlider.value.ToString();
        if ((int)widthSlider.value > currentWidth)
        {
            expandMap(true, (int)widthSlider.value);
            currentWidth = (int)widthSlider.value;

        }
        else if ((int)heightSlider.value > currentHeight)
        {
            expandMap(false, (int)heightSlider.value);
            currentHeight = (int)heightSlider.value;
        }
        else if ((int)widthSlider.value < currentWidth)
        {
            trimMap(true, (int)widthSlider.value);
            currentWidth = (int)widthSlider.value;

        }
        else if ((int)heightSlider.value < currentHeight)
        {
            trimMap(false, (int)heightSlider.value);
            currentHeight = (int)heightSlider.value;
        }
        elevationValue.text = elevationSlider.value.ToString();
        highlightSelected();
        if (selectedTiles.Count == 0)
        {
            spriteImage.sprite = null;
        }
        if (selectedTiles.Count == 1 && !tileSelected)
        {
            tileSelected = true;
            spriteImage.sprite = selectedTiles[0].GetComponent<SpriteRenderer>().sprite;
        }

        
    }

    public void getSavedLevels()
    {
        savedLevels.options.Clear();
        foreach (string filename in SaveDataHelper.getNamesOfSaves())
        {
            Debug.Log(filename);
            savedLevels.options.Add(new Dropdown.OptionData(filename));
        }
    }

    public void highlightSelected()
    {
        foreach (Transform child in GameObject.Find("Highlights").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (GameObject go in selectedTiles)
        {
            GridPosition position = go.GetComponent<TileData>().position;
            createHighlight(position.x, position.y, position.elevation);
        }
    }

    public void trimMap(bool trimWidth, int newValue)
    {

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Tile"))
        {
            GridPosition position = go.GetComponent<TileData>().position;
            if (trimWidth)
            {
                if (position.x > newValue - 1)
                {
                    Destroy(go);
                }
            }
            else
            {
                if (position.y > newValue - 1)
                {
                    Destroy(go);
                }
            }
        }
    }

    public void expandMap(bool expandWidth, int newValue)
    {
        if (expandWidth)
        {
            for (int i = 0; i < currentHeight; i++)
            {

                createBasicTile(newValue - 1, i);
            }
        }
        else
        {
            for (int i = 0; i < currentWidth; i++)
            {

                createBasicTile(i, newValue - 1);
            }
        }
    }


    public void generateBasicFlatMap(int width, int height)
    {
        for (int i = 0; i <= width; i++)
        {
            for (int j = 0; j <= height; j++)
            {
                createBasicTile(i, j);

            }
        }

    }

    public void generateTilesFromMapData(MapData data)
    {
        int maxX = 0;
        int maxY = 0;
        foreach (Transform child in GameObject.Find("Tiles").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (TileSave ts in data.tiles)
        {
            if(maxX < ts.position.x)
            {
                maxX = ts.position.x;
            }
            if (maxY < ts.position.y)
            {
                maxY = ts.position.y;
            }
            createTileFromTileSave(ts);
        }
        Debug.Log("Max values:" + maxX + "," + maxY);
        updateUIFeilds(maxX, maxY, data.mapName);
    }

    public void updateUIFeilds(int maxX, int maxY, string saveName)
    {
        selectedTiles.Clear();
        widthSlider.value = maxX + 1;
        heightSlider.value = maxY + 1;
        currentWidth = maxX + 1;
        currentHeight = maxY + 1;
        levelName.text = saveName;
    }

    public void createBasicTile(int x, int y)
    {
        GridPosition pos = new GridPosition(x, y, 0);
        GameObject go = (GameObject)Instantiate(Resources.Load("Tile"));
        go.GetComponent<TileData>().position = pos;
        go.GetComponent<TileData>().safeToStand = true;
        go.name = "x" + x + "y" + y;
        go.transform.position = IsometricHelper.gridToGamePostion(pos);
        go.GetComponent<SpriteRenderer>().sortingOrder = IsometricHelper.getTileSortingOrder(pos);
        go.transform.SetParent(GameObject.Find("Tiles").transform);
    }

    public void createTileFromTileSave(TileSave ts)
    {
        Debug.Log(ts.spriteName);
        GridPosition pos = new GridPosition(ts.position.x, ts.position.y, ts.position.elevation);
        Sprite sp = Resources.Load<Sprite>(ts.spriteName);
        GameObject go = (GameObject)Instantiate(Resources.Load("Tile"));
        go.GetComponent<TileData>().position = pos;
        go.GetComponent<TileData>().sprite = sp;
        go.GetComponent<TileData>().safeToStand = ts.safeToStand;
        go.name = "x" + pos.x + "y" + pos.y;
        go.transform.position = IsometricHelper.gridToGamePostion(pos);
        go.GetComponent<SpriteRenderer>().sortingOrder = IsometricHelper.getTileSortingOrder(pos);
        go.transform.SetParent(GameObject.Find("Tiles").transform);
    }

    public void createHighlight(int x, int y, int elevation)
    {
        GridPosition pos = new GridPosition(x, y, elevation);
        GameObject go = (GameObject)Instantiate(Resources.Load("Highlight"));
        go.name = "x" + x + "y" + y;
        go.transform.position = IsometricHelper.gridToGamePostion(pos);
        go.GetComponent<SpriteRenderer>().sortingOrder = IsometricHelper.getTileSortingOrder(pos);
        go.transform.SetParent(GameObject.Find("Highlights").transform);
    }

    public void switchSprite()
    {
        Sprite currentImage = spriteType.options[spriteType.value].image;
        spriteImage.sprite = currentImage;
    }

    public void updateSelectedTile()
    {
        foreach(GameObject go in selectedTiles)
        {
            go.GetComponent<TileData>().sprite = spriteType.options[spriteType.value].image;
            go.GetComponent<TileData>().position.elevation = (int)elevationSlider.value;
        }
        highlightSelected();
    }


    public void saveLevel()
    {
        if (!levelName.text.Equals(""))
        {
            SaveDataHelper.saveFile(GameObject.Find("Tiles").GetComponent<MapData>(), levelName.text);
        } else
        {
            SaveDataHelper.saveFile(GameObject.Find("Tiles").GetComponent<MapData>(), "New Level");
        }
        getSavedLevels();

    }

    public void loadLevel()
    {
        string filename = savedLevels.options[savedLevels.value].text;
        MapData md = SaveDataHelper.loadMapData(filename);
        generateTilesFromMapData(md);
        
    }

    
}
