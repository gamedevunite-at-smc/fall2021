using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;



[System.Serializable]
public enum Direction
{
    None, UpRight, UpLeft, DownRight, DownLeft
}

public class BaseTile : Tile
{

    public bool isRamp;


    public Direction rampDirection;

    //public void BaseTile(Tile tile)
    //{
    //    //if(tile.sprite == "rampSPrite")
    //    //{
    //    //    isRamp = true;
    //    //}
    //    //else
    //    //{
    //    //    isRamp = false;
    //    //}
    //}
}
