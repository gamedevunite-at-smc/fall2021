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
        if(startMovement != null && endMovement != null && tileMap != null)
        {
            var path = AStar.FindPath(startMovement.tilePosition, endMovement.tilePosition/*tileMap.WorldToCell(from), tileMap.WorldToCell(to)*/);

            if (path != null)
            {

                for (int i = 1; i < path.Length; i++)
                {
                    Vector3 offset = new Vector3(0.0f, 0.25f * path[i - 1].z, 0.0f);

                    Vector3 offset2 = new Vector3(0.0f, 0.25f * path[i].z, 0.0f);

                    Gizmos.DrawLine(tileMap.GetCellCenterWorld(path[i - 1]) + offset, tileMap.GetCellCenterWorld(path[i]) + offset2);
                }
            }
            else
            {
                UnityEngine.Debug.Log("This is NULL");
                Gizmos.DrawLine(tileMap.GetCellCenterWorld(startMovement.tilePosition), tileMap.GetCellCenterWorld(endMovement.tilePosition));
            }
        }

        if (showConnections && Application.isPlaying)
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
