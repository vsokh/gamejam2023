using System;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState : sbyte
{
    Start = 0,
    Finish,
    Open,
    Closed
}

public class MazeGenerator
{
    private int _height;
    private int _width;
    private int _paths;
    private NodeState[][] _maze;

    public MazeGenerator(int h = 10, int w = 10, int paths = 1)
    {
        _height = h;
        _width = w;
        _paths = paths;
        _maze = new NodeState[_height][];
        for (int i = 0; i < _height; i++)
        {
            _maze[i] = new NodeState[_width];
            for (int j = 0; j < _width; j++)
            {
                _maze[i][j] = NodeState.Closed;
            }
        }
    }

    public NodeState[][] Generate()
    {
        int startX = new System.Random().Next(_height);
        int startY = new System.Random().Next(_width);

        int finishX = new System.Random().Next(_height);
        int finishY = new System.Random().Next(_width);

        _maze[startX][startY] = NodeState.Start;
        _maze[finishX][finishY] = NodeState.Finish;

        bool[][] visited = new bool[_height][];
        for (int i = 0; i < _height; i++)
        {
            visited[i] = new bool[_width];
            for (int j = 0; j < _width; j++)
            {
                visited[i][j] = false;
            }
        }
        for (int pathIdx = 0; pathIdx < _paths; pathIdx++)
        {
            GenerateImpl(startX, startY, finishX, finishY, ref visited);
        }
        return _maze;
    }

    public void Print()
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                switch (_maze[i][j])
                {
                    case NodeState.Open: Console.Write("-"); break;
                    case NodeState.Closed: Console.Write("#"); break;
                    case NodeState.Start: Console.Write("S"); break;
                    case NodeState.Finish: Console.Write("F"); break;
                };
            }
            Console.WriteLine();
        }
    }

    private bool GenerateImpl(int currX, int currY, int finishX, int finishY, ref bool[][] visited)
    {
        if (currX < 0 || currX >= _width)  return false;
        if (currY < 0 || currY >= _height) return false;
        if (visited[currX][currY])         return false;

        // We have found a path, let's get back
        if (currX == finishX && currY == finishY) return true;

        visited[currX][currY] = true;

        if (_maze[currX][currY] != NodeState.Start) {
            _maze[currX][currY] = NodeState.Open;
        }

        List<(int,int)> neighbors = GetNeighbors(currX, currY, ref visited);
        int N = neighbors.Count-1;
        for (int idx = 0; idx < N; ++idx) {
            var neighbor = neighbors[UnityEngine.Random.Range(0, N)];
            if (!visited[neighbor.Item1][neighbor.Item2] && GenerateImpl(neighbor.Item1, neighbor.Item2, finishX, finishY, ref visited)) {
                return true;
            }
        }
        return false;
    }

    private List<(int,int)> GetNeighbors(int currX, int currY, ref bool[][] visited)
    {
        List<(int,int)> neighbors = new List<(int,int)>();
        if (currX - 1 >= 0 && !visited[currX-1][currY]) { // left
            neighbors.Add((currX-1, currY));
        }
        if (currX + 1 < _width && !visited[currX+1][currY]) { // right
            neighbors.Add((currX+1, currY));
        }
        if (currY - 1 >= 0 && !visited[currX][currY-1]) { // up
            neighbors.Add((currX, currY-1));
        }
        if (currY + 1 < _height && !visited[currX][currY+1]) { // down
            neighbors.Add((currX, currY+1));
        }
        return neighbors;
    }
}
