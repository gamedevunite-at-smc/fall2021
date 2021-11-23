using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            movement.Move(Direction.UpLeft);
            spriteFlipper.Look(Direction.UpLeft);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            movement.Move(Direction.DownRight);
            spriteFlipper.Look(Direction.DownRight);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            movement.Move(Direction.UpRight);
            spriteFlipper.Look(Direction.UpRight);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            movement.Move(Direction.DownLeft);
            spriteFlipper.Look(Direction.DownLeft);
        }
    }
}
