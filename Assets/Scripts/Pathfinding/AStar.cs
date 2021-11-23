using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{

	public static List<Node> nodes = new List<Node>();

	private static Dictionary<Vector3Int, Node> positionToNode = new Dictionary<Vector3Int, Node>();

	private static List<Node> openSet = new List<Node>();
	private static HashSet<Node> closedSet = new HashSet<Node>();

	private static List<Node> retracedPath = new List<Node>();

	private void Awake()
    {
		Tilemap tilemap = GetComponent<Tilemap>();

		foreach (var position in tilemap.cellBounds.allPositionsWithin)
		{
			if (tilemap.HasTile(position))
			{
				UnityEngine.Debug.Log(position);

				Node node = new Node();
				node.cellPosition = position;
				nodes.Add(node);
				positionToNode.Add(position, node);
			}
		}

        for (int i = 0; i < nodes.Count; i++)
        {
			nodes[i].neighbouringNodes = GetNeighbours(tilemap, nodes[i].cellPosition);
		}

		for (int i = nodes.Count - 1; i >= 0; i--)
		{
			if(nodes[i].neighbouringNodes.Length == 0)
            {
				positionToNode.Remove(nodes[i].cellPosition);
				nodes.RemoveAt(i);
			}
		}
	}

	private static Node[] GetNeighbours(Tilemap tilemap, Vector3Int position)
    {
		List<Node> neighbourNodes = new List<Node>(4);

		Node node;

		var tile = tilemap.GetTile<BaseTile>(position);

		GetNode(new Vector3Int(position.x + 1, position.y, position.z), tile.isRamp);
		GetNode(new Vector3Int(position.x - 1, position.y, position.z), tile.isRamp);
		GetNode(new Vector3Int(position.x, position.y + 1, position.z), tile.isRamp);
		GetNode(new Vector3Int(position.x, position.y - 1, position.z), tile.isRamp);

		void GetNode(Vector3Int position, bool canCheckLower)
        {
			//Check from top to bottom. If there is a tile on the top then we cannot even do the others
			if (positionToNode.TryGetValue(new Vector3Int(position.x, position.y, position.z + 2), out node))
			{
				if (tilemap.GetTile<BaseTile>(new Vector3Int(position.x, position.y, position.z + 2)).isRamp)
                {
					neighbourNodes.Add(node);
				}
			}
			//Check if there is a normal level tile in front of us
			else if (positionToNode.TryGetValue(new Vector3Int(position.x, position.y, position.z), out node))
			{
				neighbourNodes.Add(node);
			}
			//If we can check a level below, then check lol
			else if (canCheckLower && positionToNode.TryGetValue(new Vector3Int(position.x, position.y, position.z - 1), out node))
            {
				neighbourNodes.Add(node);
			}
		}

		return neighbourNodes.ToArray();
	}

    public static Vector3Int[] FindPath(Vector3Int start, Vector3Int end)
    {

        Node startNode;
        Node endNode;

        if (!positionToNode.TryGetValue(start, out startNode))
        {
			UnityEngine.Debug.Log("I CANT FIND THE START POINT!");
			return null;
		}

        if (!positionToNode.TryGetValue(end, out endNode))
		{
			UnityEngine.Debug.Log("I CANT FIND THE END POINT!");
			return null;
		}

		openSet.Clear();
		closedSet.Clear();

        openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == endNode)
			{
				RetracePath(startNode, endNode);

				return NodeListToCellPositions(retracedPath);
			}

			foreach (Node neighbour in node.neighbouringNodes)
			{
				if (closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, endNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}

		return null;
	}

	private static void RetracePath(Node startNode, Node endNode)
	{
		retracedPath.Clear();

		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			retracedPath.Add(currentNode);
			currentNode = currentNode.parent;
		}

		retracedPath.Add(startNode);

		retracedPath.Reverse();
	}

	private static int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.cellPosition.x - nodeB.cellPosition.x);
		int dstY = Mathf.Abs(nodeA.cellPosition.y - nodeB.cellPosition.y);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}

	private static Vector3Int[] NodeListToCellPositions(List<Node> nodes)
    {
		Vector3Int[] positions = new Vector3Int[nodes.Count];

        for (int i = 0; i < nodes.Count; i++)
        {
			positions[i] = nodes[i].cellPosition;
		}

		return positions;
	}

	[System.Obsolete]
	public static Node GetRandomNode()
    {
		return nodes[UnityEngine.Random.Range(0, nodes.Count)];
    }
}
