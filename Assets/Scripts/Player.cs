using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Movement _movement;
    public Movement movement => _movement;

    private SpriteFlipper spriteFlipper;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        spriteFlipper = GetComponentInChildren<SpriteFlipper>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _movement.Move(Direction.UpLeft);
            spriteFlipper.Look(Direction.UpLeft);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _movement.Move(Direction.DownRight);
            spriteFlipper.Look(Direction.DownRight);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _movement.Move(Direction.UpRight);
            spriteFlipper.Look(Direction.UpRight);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _movement.Move(Direction.DownLeft);
            spriteFlipper.Look(Direction.DownLeft);
        }
    }
}
