using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator: MonoBehaviour
{
	public NodeState[][] Generate(int dimentions, int roadsNum)
	{
		int randH1 = Random.Range(dimentions / 2, dimentions);
		int randH2 = Random.Range(0, dimentions / 2);
		if (randH1 == randH2)
			randH2 = Random.Range(0, dimentions /2);
		Vector2Int start = new Vector2Int(0, randH1);
		Vector2Int finish = new Vector2Int(dimentions - 1, randH2);

		LinkedList<Vector2Int>[] roads = new LinkedList<Vector2Int>[roadsNum];
		for (int i = 0; i < roadsNum; i++)
		{
			roads[i] = GenerateRoad(dimentions, start, finish);
		}
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
		maze[start.x][start.y] = NodeState.Start;
		maze[finish.x][finish.y] = NodeState.Finish;
		foreach (LinkedList<Vector2Int> road in roads)
		{
			foreach (Vector2Int step in road)
			{
				if (maze[step.x][step.y] != NodeState.Start && maze[step.x][step.y] != NodeState.Finish)
					maze[step.x][step.y] = NodeState.Open;
			}
		}
		return maze;
	}

	private LinkedList<Vector2Int> GenerateRoad(int dimentions, Vector2Int start, Vector2Int finish)
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
		maze[start.x][start.y] = NodeState.Start;
		maze[finish.x][finish.y] = NodeState.Finish;
		Vector2Int currentPos = start;
		Vector2Int newPos;
		LinkedList<Vector2Int> road = new LinkedList<Vector2Int>();
		road.AddFirst(currentPos);
		int kek = 0;
		while (currentPos != finish || kek > 10000)
		{
			List<Vector2Int> currentNeis = GetNeighbors(currentPos, road.Last.Previous, dimentions);
			//Debug.Log(currentNeis.Count);
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
			if (newPos != finish)
				maze[newPos.x][newPos.y] = NodeState.Open;
			road.AddLast(newPos);
			currentPos = newPos;
			kek++;
		}
		return road;
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
