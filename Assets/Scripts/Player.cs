using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.Move(Direction.UpLeft);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.Move(Direction.DownRight);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.Move(Direction.UpRight);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.Move(Direction.DownLeft);
        }
    }
}
