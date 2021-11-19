using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Movement movement;

    private SpriteFlipper spriteFlipper;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        spriteFlipper = GetComponentInChildren<SpriteFlipper>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.Move(Direction.UpLeft);
            spriteFlipper.Look(Direction.UpLeft);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.Move(Direction.DownRight);
            spriteFlipper.Look(Direction.DownRight);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.Move(Direction.UpRight);
            spriteFlipper.Look(Direction.UpRight);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.Move(Direction.DownLeft);
            spriteFlipper.Look(Direction.DownLeft);
        }
    }
}
