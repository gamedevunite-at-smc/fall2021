using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{

    private new Transform transform;
    private Tilemap tileMap;

    private Vector3Int tilePosition;

    void Start()
    {
        transform = GetComponent<Transform>();

        tileMap = GameObject.Find("TestMap").GetComponent<Tilemap>();

        tilePosition = tileMap.WorldToCell(transform.position);
        transform.position = tileMap.CellToWorld(tilePosition);
        tilePosition.z = 1;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Direction.UpLeft);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Direction.DownRight);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Direction.UpRight);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Direction.DownLeft);
        }
    }

    //Maybe some information on the tile?
    public delegate void OnMoveDelegate(Direction direction, Vector3Int cellPosition);
    public event OnMoveDelegate OnSuccessfullMove;
    public event OnMoveDelegate OnFailedMove;

    public void Move(Direction direction)
    {
	    Vector3Int originalTilePosition = tilePosition;

        switch (direction)
        {
            case Direction.None:
                break;
            case Direction.UpRight:
		        Debug.Log("UpRight");
                tilePosition.x++;
                break;
            case Direction.UpLeft:
		        Debug.Log("UpLeft");
                tilePosition.y++;
                break;
            case Direction.DownRight:
		        Debug.Log("DownRight");
                tilePosition.y--;
                break;
            case Direction.DownLeft:
		        Debug.Log("DownLeft");
                tilePosition.x--;
                break;
            default:
                break;
        }

        Vector3Int aboveTilePosition = new Vector3Int(tilePosition.x, tilePosition.y, tilePosition.z + 1);
        Vector3Int belowTilePosition = new Vector3Int(tilePosition.x, tilePosition.y, tilePosition.z - 1);

        int onRamp = 0;

        //from base later to 1 layer up
        if (tileMap.HasTile(aboveTilePosition))
        {
            UnityEngine.Debug.Log("There is a tile here");

            var aboveTile = tileMap.GetTile<BaseTile>(aboveTilePosition);

            if (aboveTile.isRamp)
            {
		        UnityEngine.Debug.Log("This tile is a ramp");
                tilePosition.z += 1;
                onRamp = -1;
            }
            else
            {
                //we cant move on this tile
                tilePosition = originalTilePosition;
            }
        } 
        else if (tileMap.HasTile(tilePosition))
        {
            var belowTile = tileMap.GetTile<BaseTile>(tilePosition);

            if (belowTile.isRamp)
            {
                tilePosition.z -= 1;
                onRamp = 1;
            }
        } 
        else if (!tileMap.HasTile(belowTilePosition))
        {
		    //No tile under our feet. Can we do a fun animation here?
		    tilePosition = originalTilePosition;
            //We dont move dont call event
	    }

        Vector3 offset = new Vector3(0.0f, 0.25f * tilePosition.z, 0.0f);

        offset.y += .25f * onRamp;

        transform.position = tileMap.GetCellCenterWorld(tilePosition) + offset;
    }
}
