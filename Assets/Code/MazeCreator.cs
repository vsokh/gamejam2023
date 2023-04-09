using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator: MonoBehaviour
{
	public NodeState[][] Generate(int dimentions)
	{
		NodeState[][] maze;

		maze = new NodeState[dimentions][];
		for (int i = 0; i < dimentions; i++)
		{
			maze[i] = new NodeState[dimentions];
			for (int j = 0; j < dimentions; j++)
			{
				maze[i][j] = NodeState.Closed;
			}
		}

		Vector2Int start = new Vector2Int(0, Random.Range(0, dimentions));
		Vector2Int finish = new Vector2Int(dimentions - 1, Random.Range(0, dimentions));

		maze[start.x][start.y] = NodeState.Start;
		maze[finish.x][finish.y] = NodeState.Finish;

		Vector2Int currentPos = start;
		Vector2Int newPos;
		LinkedList<Vector2Int> road = new LinkedList<Vector2Int>();
		road.AddFirst(currentPos);
		int kek = 0;
		while (kek < 10)
		{
			List<Vector2Int> currentNeis = GetNeighbors(currentPos, road.Last.Previous, dimentions);
			Debug.Log(currentNeis.Count);
			newPos = currentNeis[Random.Range(0, currentNeis.Count)];
			List<Vector2Int> newNeis = GetNeighbors(newPos, road.Last, dimentions);
			foreach (Vector2Int newNeighbor in newNeis)
			{
				if (road.Contains(newNeighbor))
				{
					while (road.Last.Value != newNeighbor)
					{
						Vector2Int discarded = road.Last.Value;
						if (discarded == start || discarded == finish) break;
						road.RemoveLast();
						maze[discarded.x][discarded.y] = NodeState.Closed;
					}
				}
			}
			maze[newPos.x][newPos.y] = NodeState.Open;
			road.AddLast(newPos);
			currentPos = newPos;
			kek++;
		}
		return maze;
	}

	


	private List<Vector2Int> GetNeighbors(Vector2Int current, LinkedListNode<Vector2Int> prev, int dimentions)
	{

		List<Vector2Int> neighbors = new List<Vector2Int>();
		if (current.x - 1 >= 0)
		{
			Vector2Int neighbor = new Vector2Int(current.x - 1, current.y);
			if (prev == null || (prev != null && prev.Value != neighbor)) neighbors.Add(neighbor);
		}
		if (current.x + 1 < dimentions)
		{
			Vector2Int neighbor = new Vector2Int(current.x + 1, current.y);
			if (prev == null || (prev != null && prev.Value != neighbor)) neighbors.Add(neighbor);
		}
		if (current.y - 1 >= 0)
		{
			Vector2Int neighbor = new Vector2Int(current.x, current.y - 1);
			if (prev == null || (prev != null && prev.Value != neighbor)) neighbors.Add(neighbor);
		}
		if (current.y + 1 < dimentions)
		{
			Vector2Int neighbor = new Vector2Int(current.x, current.y + 1);
			if (prev == null || (prev != null && prev.Value != neighbor)) neighbors.Add(neighbor);
		}
		return neighbors;
	}

}
