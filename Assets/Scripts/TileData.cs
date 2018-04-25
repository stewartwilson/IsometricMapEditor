using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour {

    public GridPosition position;
    public Sprite sprite;
    public bool safeToStand;

    public int lastElevation;
    public bool multipleSelect = false;

    public TileSave tileSave;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        tileSave = new TileSave();
        updateTileSave();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Multi Select"))
        {
            multipleSelect = true;
        }
        if (Input.GetButtonUp("Multi Select"))
        {
            multipleSelect = false;
        }
        if (lastElevation != position.elevation) {
            transform.position = IsometricHelper.gridToGamePostion(position);
            lastElevation = position.elevation;
        }
        GetComponent<SpriteRenderer>().sprite = sprite;
        updateTileSave();
    }

    void OnMouseDown()
    {
        if (GameObject.Find("Tools").GetComponent<ToolsController>().selectedTiles.Contains(gameObject))
        {
            GameObject.Find("Tools").GetComponent<ToolsController>().selectedTiles.Remove(gameObject);
        }
        else
        {
            if (multipleSelect)
            {
                GameObject.Find("Tools").GetComponent<ToolsController>().selectedTiles.Add(gameObject);
            }
            else
            {
                GameObject.Find("Tools").GetComponent<ToolsController>().selectedTiles.Clear();
                GameObject.Find("Tools").GetComponent<ToolsController>().selectedTiles.Add(gameObject);
            }
        }
    }

    public void updateTileSave()
    {
        tileSave.position = position;
        tileSave.spriteName = sprite.name;
        tileSave.safeToStand = safeToStand;
    }
}
