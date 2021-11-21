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

    private int direction;
    /*
     * This variable, and my idea of movement, explained
     * 
     * This variable stores the direction of movement
     * 
     * Last four bits
     * XXYY
     * 
     * X bits - timer 0.75
     * 01-- (decimal 5, 6, 7)       DownLeft
     * 10-- (decimal 9, 10, 11)     None
     * 11-- (decimal 13, 14, 15)    UpRight
     * 
     * Y bits - timer 1.5
     * --01 (decimal 5, 9, 13)      DownRight
     * --10 (decimal 6, 10, 14)     None
     * --11 (decimal 7, 11, 15)     UpLeft
     * 
     * This is extremely janky
     * 
     */

    //More variables for the movement system
    private float moveTimer;
    private bool halfSurpassed;
    public float maxTime = 1.0f;
    private float halfTime;

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

        direction = 0b1010; //Numbers preceded by 0b are in binary
        moveTimer = 0.0f;
        halfSurpassed = false;
        halfTime = maxTime / 2;
    }

    private void Update()
    {
        int newDirection = Math.Sign(Input.GetAxis("Vertical")) + 2; //Set X bits
        newDirection |= (Math.Sign(Input.GetAxis("Horizontal")) + 2) << 2;
        //Debug.Log(newDirection);
        if(newDirection != direction)
        {
            //Reset the timer
            direction = newDirection;
            moveTimer = 0.0f;
            halfSurpassed = false;
        }
        else
        {
            moveTimer += Time.deltaTime;
        }

        //Movement in the X Direction
        if(moveTimer > halfTime && !halfSurpassed)
        {
            halfSurpassed = true;
            //& is the bitwise and operator, which is different from the logical and operator. Basically, & filters out bits
            switch(direction & 0b1100)
            {
                case 0b0100:
                    Move(Direction.DownLeft);
                    break;
                case 0b1100:
                    Move(Direction.UpRight);
                    break;
                case 0b1000:
                    switch(direction & 0b0011)
                    {
                        case 0b0001:
                            Move(Direction.DownRight);
                            break;
                        case 0b0011:
                            Move(Direction.UpLeft);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        //Movement in the Y Direction
        if(moveTimer > maxTime)
        {
            //Reset the timer
            moveTimer = 0.0f;
            halfSurpassed = false;
            switch(direction & 0b0011)
            {
                case 0b0001:
                    Move(Direction.DownRight);
                    break;
                case 0b0011:
                    Move(Direction.UpLeft);
                    break;
                case 0b0010:
                    switch(direction & 0b1100)
                    {
                        case 0b0100:
                            Move(Direction.DownLeft);
                            break;
                        case 0b1100:
                            Move(Direction.UpRight);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public Direction OppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.None:
                return Direction.None;
            case Direction.UpRight:
                return Direction.DownLeft;
            case Direction.DownLeft:
                return Direction.UpRight;
            case Direction.UpLeft:
                return Direction.DownRight;
            case Direction.DownRight:
                return Direction.UpLeft;
            default:
                return Direction.None;
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
                Vector3Int positionAboveUs = new Vector3Int(originalTilePosition.x, originalTilePosition.y, originalTilePosition.z+1);
                var tileAboveUs = tileMap.GetTile<BaseTile>(positionAboveUs);
                //previousTile = null;
                Debug.Log(tileAboveUs);
                if (tileAboveUs != null)
                {
                    if (tileAboveUs.isRamp && tileAboveUs.rampDirection == direction)
                    {
                        tilePosition.z = originalTilePosition.z + 1;
                        onRamp = 0;
                    }
                    else
                    {
                        //we cant move on this tile
                        tilePosition = originalTilePosition;
                        movementStatus = MovementStatus.Failed;
                    }
                }
                else
                {
                    //we cant move on this tile
                    tilePosition = originalTilePosition;
                    movementStatus = MovementStatus.Failed;
                }
            }
        } 
        else if (tileMap.HasTile(tilePosition))
        {
            var belowTile = tileMap.GetTile<BaseTile>(tilePosition);

            if (belowTile.isRamp)
            {
                Debug.Log(belowTile.rampDirection);
                tilePosition.z  -= 1;
                onRamp = 1;
            }
        } 
        else if (!tileMap.HasTile(belowTilePosition))
        {
            //Vector3Int positionBelowUs = new Vector3Int(originalTilePosition.x, originalTilePosition.y, originalTilePosition.z - 1);

            var tileBelowUs = tileMap.GetTile<BaseTile>(originalTilePosition);
            
            Debug.Log(originalTilePosition.z);
            if (tileBelowUs != null)
            {
                if (tileBelowUs.isRamp && tileBelowUs.rampDirection == OppositeDirection(direction))
                {
                    tilePosition.z = originalTilePosition.z - 1;
                    onRamp = 0;
                }
                else
                {
                    movementStatus = MovementStatus.Failed;
                    //No tile under our feet. Can we do a fun animation here?
                    tilePosition = originalTilePosition;
                    //We dont move dont call event
                }
            }
            else
            {
                movementStatus = MovementStatus.Failed;
                //No tile under our feet. Can we do a fun animation here?
                tilePosition = originalTilePosition;
                //We dont move dont call event
            }



        }


        Vector3 offset = new Vector3(0.0f, 0.25f * tilePosition.z, 0.0f);
        offset.y += .25f * onRamp;

        transform.position = tileMap.GetCellCenterWorld(tilePosition) + offset;

        Debug.Log(originalTilePosition.z);

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
