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

public class MazeGenerator : MonoBehaviour
{
    private int _height;
    private int _width;
    private int _paths;
    private NodeState[][] _maze;

    private void Init(int h, int w, int paths)
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

    private bool[][] NewVisited()
    {
        bool[][] visited = new bool[_height][];
        for (int i = 0; i < _height; i++)
        {
            visited[i] = new bool[_width];
            for (int j = 0; j < _width; j++)
            {
                visited[i][j] = false;
            }
        }
        return visited;
    }

    private void ClearVisited(ref bool[][] visited)
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                visited[i][j] = false;
            }
        }
    }

    // ((startX, startY), (finishX, finishY)))
    private ((int, int), (int, int)) GenerateStartAndFinish()
    {
        System.Random rand = new System.Random();
        return ((0, rand.Next((_height-1))), (_width-1, rand.Next((_height-1))));
    }

    public NodeState[][] Generate(int h = 5, int w = 5, int paths = 1)
    {
        Init(h, w, paths);

        // ((startX, startY), (finishX, finishY)))
        ((int, int), (int, int)) pos = GenerateStartAndFinish();

        _maze[pos.Item1.Item1][pos.Item1.Item2] = NodeState.Start;
        _maze[pos.Item2.Item1][pos.Item2.Item2] = NodeState.Finish;

        bool[][] visited = NewVisited();
        for (int pathIdx = 0; pathIdx < _paths; pathIdx++)
        {
            ClearVisited(ref visited);
            GenerateImpl(pos.Item1.Item1, pos.Item1.Item2, pos.Item2.Item1, pos.Item2.Item2, ref visited);
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
        if (_maze[currX][currY] == NodeState.Finish)
        {
            return true;
        }

        visited[currX][currY] = true;

        if (_maze[currX][currY] != NodeState.Start) {
            _maze[currX][currY] = NodeState.Open;
        }

        List<(int,int)> neighbors = GetNeighbors(currX, currY, ref visited);
        int N = neighbors.Count;
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
