using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{

    private new Transform transform;

    private Tilemap tileMap;

    void Start()
    {
        transform = GetComponent<Transform>();

        tileMap = GameObject.Find("TerrainMap").GetComponent<Tilemap>();
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(-1, 0, false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(1, 0, false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(0, 1, false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(0, -1, false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(0, -1, false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Move(0, -1, true);
        }
    }

    //public void UploadData()
    //{

    //    var baseTile = new BaseTile();
    //    baseTile.isRamp = false;
    //    baseTile.rampDirection = BaseTile.RampDirection.UpRight;
    //}

    public void Move(int x, int y, bool isRamp, BaseTile.RampDirection rampDirection = BaseTile.RampDirection.UpRight)
    {

        Vector3 offsetPosition = Vector3.zero;

        offsetPosition += x * new Vector3(0.5f, 0.25f, 0);
        offsetPosition += y * new Vector3(0.5f, -0.25f, 0);

        var tile = tileMap.GetTile(tileMap.WorldToCell(transform.position + offsetPosition));

        if (/*tile.isRamp*/isRamp)
        {
            switch (/*tile.*/rampDirection)
            {
                case BaseTile.RampDirection.UpRight:
                    offsetPosition.y += 0.25f;
                    break;
                case BaseTile.RampDirection.UpLeft:
                    offsetPosition.y -= 0.25f;
                    break;
                case BaseTile.RampDirection.DownRight:
                    offsetPosition.y -= 0.25f;
                    break;
                case BaseTile.RampDirection.DownLeft:
                    offsetPosition.y += 0.25f;
                    break;
                default:
                    break;
            }
        }

        transform.position += offsetPosition;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
