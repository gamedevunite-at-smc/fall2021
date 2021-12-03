using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Player player;

    //The enemy unit's movement script
    private Movement _movement;

    void Start()
    {
        player = FindObjectOfType<Player>();
        _movement = GetComponent<Movement>();

        InvokeRepeating("MoveOneSpace", 1.0f, 1.0f);
    }

    private void MoveOneSpace()
    {
        MoveTowardPlayer(1);
    }

    private void MoveTowardPlayer(int tileCount)
    {
        var path = AStar.FindPath(player.movement.tilePosition, _movement.tilePosition);

        for (int i = 0; i < tileCount; i++)
        {
            var direction = path[i] - _movement.tilePosition;

            if(direction.x != 0)
            {
                if (direction.x > 0)
                    _movement.Move(Direction.UpRight);
                else
                    _movement.Move(Direction.DownLeft);
            }
            if(direction.y != 0)
            {
                if (direction.y > 0)
                    _movement.Move(Direction.UpLeft);
                else
                    _movement.Move(Direction.DownRight);
            }
        }
    }

}
