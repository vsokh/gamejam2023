using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator: MonoBehaviour
{
	public enum NodeState : short
	{
		Start = 0,
		Finish,
		Open,
		Closed
	};

	public object Generate(int dimentions)
	{
		NodeState[,] maze = new NodeState[dimentions, dimentions];

		maze[Random.Range(0, dimentions), Random.Range(0, dimentions)] = NodeState.Closed;
		maze[Random.Range(0, dimentions), Random.Range(0, dimentions)] = NodeState.Closed;
		maze[Random.Range(0, dimentions), Random.Range(0, dimentions)] = NodeState.Closed;
		maze[Random.Range(0, dimentions), Random.Range(0, dimentions)] = NodeState.Closed;

		return maze;
	}

}
