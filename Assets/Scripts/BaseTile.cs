using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;



public class BaseTile : Tile
{

    public bool isRamp;

    public enum RampDirection 
    { 
        UpRight, UpLeft, DownRight, DownLeft
    }

    public RampDirection rampDirection;

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
