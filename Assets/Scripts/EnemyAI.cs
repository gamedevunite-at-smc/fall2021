using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private Movement movement;
    private SpriteFlipper spriteFlipper;

    private Direction dir = Direction.DownRight;

    private float moveTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        spriteFlipper = GetComponentInChildren<SpriteFlipper>();
        movement.OnFailedMove += turnAround;

        //movement.setZCoord(2);
    }

    // Update is called once per frame
    void Update()
    {
        
        moveTimer += Time.deltaTime;
        if(moveTimer > 1.0f)
        {
            moveTimer -= 1.0f;
            movement.Move(dir);
        }
    }

    public void turnAround(Direction direction, Vector3Int cellPosition)
    {
        dir = movement.OppositeDirection(direction);
        spriteFlipper.Look(dir);
    }
}
