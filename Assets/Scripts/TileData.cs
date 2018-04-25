using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileData : MonoBehaviour {

    public GridPosition position;
    public Sprite sprite;
    public bool safeToStand;

    public int lastElevation;
    public bool multipleSelect = false;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
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
    }

    void OnMouseDown()
    {
        if (multipleSelect)
        {
            GameObject.Find("Tools").GetComponent<ToolsController>().selectedTiles.Add(gameObject);
        } else
        {
            GameObject.Find("Tools").GetComponent<ToolsController>().selectedTiles.Clear();
            GameObject.Find("Tools").GetComponent<ToolsController>().selectedTiles.Add(gameObject);
        }
    }
}
