using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{

    private new Transform transform;
    private SpriteRenderer spriteRenderer;
    private Tilemap tileMap;

    private bool wasOnRamp = false;
    private Direction previousRampDirection;

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

    public void Move(Direction direction)
    {
	Vector3Int originalTilePosition = tilePosition;
        Vector3 offsetPosition = new Vector3(0.5f, 0.25f, 0);
        switch (direction)
        {
            case Direction.None:
                break;
            case Direction.UpRight:
		Debug.Log("UpRight");
                offsetPosition = new Vector3(0.5f, 0.25f, 0);
                tilePosition.x++;
                break;
            case Direction.UpLeft:
		Debug.Log("UpLeft");
                offsetPosition = new Vector3(-0.5f, 0.25f, 0);
                tilePosition.y++;
                break;
            case Direction.DownRight:
		Debug.Log("DownRight");
                offsetPosition = new Vector3(0.5f, -0.25f, 0);
                tilePosition.y--;
                break;
            case Direction.DownLeft:
		Debug.Log("DownLeft");
                offsetPosition = new Vector3(-0.5f, -0.25f, 0);
                tilePosition.x--;
                break;
            default:
                break;
        }

        //Vector3Int playerCellPosition = tileMap.WorldToCell(tilePosition/*transform.position*/);

        //UnityEngine.Debug.Log(playerCellPosition);

        //var currentTile = (BaseTile)tileMap.GetTile(tilePosition);

        //UnityEngine.Debug.Log(currentTile.isRamp);

        //if (wasOnRamp)
        //{


        //    Vector2 rampOffset = new Vector2(0.0f, 0.25f);

        //    switch (previousRampDirection)
        //    {
        //        case Direction.UpRight:

        //            if (direction == Direction.DownLeft)
        //            {
        //                rampOffset *= -1;
        //            }

        //            break;
        //        case Direction.UpLeft:

        //            if (direction == Direction.DownLeft)
        //            {
        //                rampOffset *= -1;
        //            }

        //            break;
        //        case Direction.DownRight:

        //            rampOffset *= -1;

        //            if (direction == Direction.UpLeft)
        //            {
        //                rampOffset *= -1;
        //            }

        //            break;
        //        case Direction.DownLeft:

        //            rampOffset *= -1;

        //            if (direction == Direction.UpRight)
        //            {
        //                rampOffset *= -1;
        //            }

        //            break;
        //        default:
        //            break;
        //    }

        //    offsetPosition.x += rampOffset.x;
        //    offsetPosition.y += rampOffset.y;

        //    wasOnRamp = false;
        //}

        ////Vector3Int playerCellDestination = tileMap.WorldToCell(/*transform.position + offsetPosition*/);
        //var tile = (BaseTile)tileMap.GetTile(tilePosition/*playerCellDestination*/);

        //UnityEngine.Debug.Log(tilePosition);
        //UnityEngine.Debug.Log("Tile is ramp: " + tile.isRamp);

        //if (tile.isRamp)
        //{

        //    Vector2 rampOffset = new Vector2(0.0f, 0.25f);

        //    switch (tile.rampDirection)
        //    {
        //        case Direction.UpRight:

        //            if(direction == Direction.DownLeft)
        //            {
        //                rampOffset *= -1;
        //            }

        //            break;
        //        case Direction.UpLeft:

        //            if (direction == Direction.DownLeft)
        //            {
        //                rampOffset *= -1;
        //            }

        //            break;
        //        case Direction.DownRight:

        //            rampOffset *= -1;

        //            if (direction == Direction.UpLeft)
        //            {
        //                rampOffset *= -1;
        //            }

        //            break;
        //        case Direction.DownLeft:

        //            rampOffset *= -1;

        //            if (direction == Direction.UpRight)
        //            {
        //                rampOffset *= -1;
        //            }

        //            break;
        //        default:
        //            break;
        //    }

        //    offsetPosition.x += rampOffset.x;
        //    offsetPosition.y += rampOffset.y;

        //    wasOnRamp = true;
        //    previousRampDirection = tile.rampDirection;
        //}

        Vector3Int resultingTilePosition = tilePosition;

        Vector3Int aboveTilePosition = new Vector3Int(tilePosition.x, tilePosition.y, tilePosition.z + 1);
        Vector3Int belowTilePosition = new Vector3Int(tilePosition.x, tilePosition.y, tilePosition.z - 1);

        //from base later to 1 layer up
        if (tileMap.HasTile(aboveTilePosition))
        {
            UnityEngine.Debug.Log("There is a tile here");

            var aboveTile = tileMap.GetTile<BaseTile>(aboveTilePosition);

            if (aboveTile.isRamp)
            {
		        UnityEngine.Debug.Log("This tile is a ramp");
                tilePosition.z += 1;
            }
            else
            {
                //we cant move on this tile
                tilePosition = originalTilePosition;
            }

            //end
        } 
        //Going down from base surfuse to a level lower
        else if (tileMap.HasTile(tilePosition))
        {
            var belowTile = tileMap.GetTile<BaseTile>(tilePosition);

            if (belowTile.isRamp)
            {
                tilePosition.z -= 1;
            }
            else
            {
		        UnityEngine.Debug.Log("Hello");
                //just moving forward on tile
            }
        } 
        else if (!tileMap.HasTile(belowTilePosition))
        {
		    //No tile under our feet. Can we do a fun animation here?
		    tilePosition = originalTilePosition;
	    }

        transform.position = tileMap.GetCellCenterWorld(tilePosition) + new Vector3(0.0f, 0.25f*tilePosition.z, 0.0f);
    }
}
