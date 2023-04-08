#include <cstdlib>
#include <iostream>
#include <vector>
#include <random>

enum class NodeState : int8_t
{
    Start = 0,
    Finish,
    Open,
    Closed
};

using Maze = std::vector<std::vector<NodeState>>;

class MazeGenerator
{
    int _height{0};
    int _width{0};

    int _paths{0};
    Maze _maze{};

    public:
    explicit MazeGenerator(int h = 10, int w = 10, int paths = 1)
        : _height{h}, _width{w}
        , _paths{paths}
        , _maze(_height, std::vector<NodeState>(_width, NodeState::Closed))
    {
    }

    Maze Generate()
    {
        // TODO: make sure start != finish
        int startX = 5; // rand() % _height;
        int startY = 0; // rand() % _width;

        int finishX = 0; // rand() % _height;
        int finishY = 5; // rand() % _width;

        _maze[startX][startY] = NodeState::Start;
        _maze[finishX][finishY] = NodeState::Finish;
        std::vector<std::vector<bool>> visited(_height, std::vector<bool>(_width, 0));
        for (int pathIdx = 0; pathIdx < _paths; ++pathIdx) {
            GenerateImpl(startX, startY, finishX, finishY, visited);
        }
        return _maze;
    }

    void Print()
    {
        for (auto row : _maze) {
            for (auto e : row) {
                switch (e) {
                    case NodeState::Open:   std::cout << "-"; break;
                    case NodeState::Closed: std::cout << "#"; break;
                    case NodeState::Start:  std::cout << "S"; break;
                    case NodeState::Finish: std::cout << "F"; break;
                };
            }
            std::cout << "\n";
        }
    }

private:
    bool GenerateImpl(int currX, int currY, int finishX, int finishY, std::vector<std::vector<bool>>& visited)
    {
        if (currX < 0 || currX >= _width)  return false;
        if (currY < 0 || currY >= _height) return false;
        if (visited[currX][currY])         return false;

        // We have found a path, let's get back
        if (currX == finishX && currY == finishY) return true;

        visited[currX][currY] = 1;

        if (_maze[currX][currY] != NodeState::Start) {
            _maze[currX][currY] = NodeState::Open;
        }

        std::vector<std::pair<int,int>> neighbors = GetNeighbors(currX, currY, visited);
        int N = neighbors.size();
        for (int idx = 0; idx < N; ++idx) {
            auto neighbor = neighbors[rand()%N];
            if (!visited[neighbor.first][neighbor.second] && GenerateImpl(neighbor.first, neighbor.second, finishX, finishY, visited)) {
                return true;
            }
        }
        return false;
    }

    std::vector<std::pair<int,int>> GetNeighbors(int currX, int currY, const std::vector<std::vector<bool>>& visited)
    {
        std::vector<std::pair<int,int>> neighbors;
        if (currX - 1 >= 0 && !visited[currX-1][currY]) { // left
            neighbors.push_back(std::pair<int,int>{currX-1, currY});
        }
        if (currX + 1 < _width && !visited[currX+1][currY]) { // right
            neighbors.push_back(std::pair<int,int>{currX+1, currY});
        }
        if (currY - 1 >= 0 && !visited[currX][currY-1]) { // up
            neighbors.push_back(std::pair<int,int>{currX, currY-1});
        }
        if (currY + 1 < _height && !visited[currX][currY+1]) { // down
            neighbors.push_back(std::pair<int,int>{currX, currY+1});
        }
        return neighbors;
    }
};

int main()
{
    MazeGenerator mazeGen;
    mazeGen.Generate();
    mazeGen.Print();
    return 0;
}
