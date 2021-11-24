using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarTest : MonoBehaviour
{

    private AStar aStar;
    private Tilemap tileMap;

   //public Vector3Int to;
   //public Vector3Int from;

    //private Node to;
    //private Node from;

    public bool showConnections = false;

    public Movement startMovement;
    public Movement endMovement;

    void Start()
    {
        Debug.Log("Hello");
        aStar = GetComponent<AStar>();
        tileMap = GetComponent<Tilemap>();


        //to = AStar.GetRandomNode();
        //from = AStar.GetRandomNode();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    to = AStar.GetRandomNode();
        //    from = AStar.GetRandomNode();
        //}
    }

    private void OnDrawGizmos()
    {
        //aStar.sayHello();
        if(startMovement != null && endMovement != null && tileMap != null)
        {
            var path = AStar.FindPath(startMovement.tilePosition, endMovement.tilePosition/*tileMap.WorldToCell(from), tileMap.WorldToCell(to)*/);

            if (path != null)
            {

                for (int i = 1; i < path.Length; i++)
                {
                    Gizmos.DrawLine(tileMap.GetCellCenterWorld(path[i - 1]), tileMap.GetCellCenterWorld(path[i]));
                }
            }
            else
            {
                UnityEngine.Debug.Log("This is NULL");
                Gizmos.DrawLine(tileMap.GetCellCenterWorld(startMovement.tilePosition), tileMap.GetCellCenterWorld(endMovement.tilePosition));
            }
        }

        if (showConnections)
        {
            for (int i = 0; i < AStar.nodes.Count; i++)
            {
                for (int j = 0; j < AStar.nodes[i].neighbouringNodes.Length; j++)
                {
                    Gizmos.DrawLine(tileMap.GetCellCenterWorld(AStar.nodes[i].cellPosition + new Vector3Int(1, 1, 0)), tileMap.GetCellCenterWorld(AStar.nodes[i].neighbouringNodes[j].cellPosition + new Vector3Int(1, 1, 0)));
                }
            }
        }
    }

}
