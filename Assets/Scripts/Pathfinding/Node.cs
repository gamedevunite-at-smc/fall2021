using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public float weight;

    public Vector3Int cellPosition;

    public Node parent = null;

    public Node[] neighbouringNodes;

    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;

}
