using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour {

    public string mapName;

    [SerializeField]
    public List<TileSave> tiles;

    private void Awake()
    {
        tiles = new List<TileSave>();
    }

    // Update is called once per frame
    void Update () {
        tiles.Clear();
        foreach (Transform child in GameObject.Find("Tiles").transform)
        {
            if ("Tile".Equals(child.gameObject.tag))
            {
                tiles.Add(child.gameObject.GetComponent<TileData>().tileSave);
            }
        }
    }
}
