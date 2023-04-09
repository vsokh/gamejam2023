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

	public NodeState[][] Generate(int dimentions)
	{
		NodeState[][] maze;

        maze = new NodeState[dimentions][];
        for (int i = 0; i < dimentions; i++)
        {
           maze[i] = new NodeState[dimentions];
            for (int j = 0; j < dimentions; j++)
            {
                maze[i][j] = NodeState.Open;
            }
        }

		maze[Random.Range(0, dimentions)][Random.Range(0, dimentions)] = NodeState.Closed;
		maze[Random.Range(0, dimentions)][Random.Range(0, dimentions)] = NodeState.Closed;
		maze[Random.Range(0, dimentions)][Random.Range(0, dimentions)] = NodeState.Closed;
		maze[Random.Range(0, dimentions)][Random.Range(0, dimentions)] = NodeState.Closed;

		return maze;
	}

}
