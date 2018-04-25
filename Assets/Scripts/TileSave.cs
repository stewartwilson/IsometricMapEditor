using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileSave {

    [SerializeField]
    public GridPosition position;
    [SerializeField]
    public string spriteName;
    [SerializeField]
    public bool safeToStand;

}
