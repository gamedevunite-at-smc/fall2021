using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();    
    }

    public void Look(Direction direction)
    {
        switch (direction)
        {
            case Direction.None:
                break;
            case Direction.UpRight:
                spriteRenderer.flipX = false;
                break;
            case Direction.UpLeft:
                spriteRenderer.flipX = true;
                break;
            case Direction.DownRight:
                spriteRenderer.flipX = false;
                break;
            case Direction.DownLeft:
                spriteRenderer.flipX = true;
                break;
            default:
                break;
        }
    }
}
