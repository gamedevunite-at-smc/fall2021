using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{

    private Tilemap tileMap;

    private new Transform transform;

    private Vector3Int tilePosition;

    private int moveTimer;

    private Vector2Int direction;

    //Maybe some more information from the tile?
    public delegate void OnMoveDelegate(Direction direction, Vector3Int cellPosition);
    public event OnMoveDelegate OnMove;
    public event OnMoveDelegate OnSuccessfulMove;
    public event OnMoveDelegate OnFailedMove;

    private enum MovementStatus { Sucessful, Failed }

    private void Start()
    {
        transform = GetComponent<Transform>();

        tileMap = GameObject.Find("TestMap").GetComponent<Tilemap>();

        tilePosition = tileMap.WorldToCell(transform.position);
        transform.position = tileMap.CellToWorld(tilePosition);
        tilePosition.z = 1;
        direction = new Vector2Int(0, 0);
        moveTimer = 0;
    }

    private void Update()
    {
        //Is there already movement?
        var newDirection = new Vector2Int(Math.Sign(Input.GetAxis("Horizontal")), Math.Sign(Input.GetAxis("Vertical")));
        if(newDirection.x != direction.x)
        {
            //Reset the timer
            direction.x = newDirection.x;
            moveTimer = 0;
        }
        else
        {
            moveTimer++;
        }
        //Move timer resets every 60 frames.
        if(moveTimer == 60)
        {
            //Reset the timer
            moveTimer = 0;
            switch (newDirection.x)
            {
                case 1:
                    Move(Direction.UpRight);
                    break;
                case -1:
                    Move(Direction.DownLeft);
                    break;
                default:
                    break;
            }
        }
    }

    public void Move(Direction direction)
    {
        MovementStatus movementStatus = MovementStatus.Sucessful;

        Vector3Int originalTilePosition = tilePosition;

        switch (direction)
        {
            case Direction.None:
                break;
            case Direction.UpRight:
                tilePosition.x++;
                break;
            case Direction.UpLeft:
                tilePosition.y++;
                break;
            case Direction.DownRight:
                tilePosition.y--;
                break;
            case Direction.DownLeft:
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
                movementStatus = MovementStatus.Failed;
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
            movementStatus = MovementStatus.Failed;
            //No tile under our feet. Can we do a fun animation here?
            tilePosition = originalTilePosition;
            //We dont move dont call event
	    }


        Vector3 offset = new Vector3(0.0f, 0.25f * tilePosition.z, 0.0f);
        offset.y += .25f * onRamp;

        transform.position = tileMap.GetCellCenterWorld(tilePosition) + offset;

        switch (movementStatus)
        {
            case MovementStatus.Sucessful:

                OnSuccessfulMove?.Invoke(direction, tilePosition);

                break;
            case MovementStatus.Failed:

                OnFailedMove?.Invoke(direction, tilePosition);

                break;
            default:
                break;
        }

        OnMove?.Invoke(direction, tilePosition);
    }
}
