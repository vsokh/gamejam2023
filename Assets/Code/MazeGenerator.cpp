#include <cstdlib>
#include <iostream>
#include <vector>
#include <random>

enum class NodeState : int8_t
{
    Open = 0,
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
    explicit MazeGenerator(int h = 6, int w = 6, int paths = 3)
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
                if (e == NodeState::Open) {
                    std::cout << "-";
                } else {
                    std::cout << "#";
                }
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

        _maze[currX][currY] = NodeState::Open;

        std::vector<std::pair<int,int>> neighbors = GetNeighbors(currX, currY, visited);
        for (auto neighbor : neighbors) {
            if (GenerateImpl(neighbor.first, neighbor.second, finishX, finishY, visited)) {
                return true;
            }
        }
        return false;
    }

    std::vector<std::pair<int,int>> GetNeighbors(int currX, int currY, const std::vector<std::vector<bool>>& visited)
    {
        std::vector<std::pair<int,int>> neighbors;
        if (currX - 1 >= 0 && currY && visited[currX-1][currY]) { // left
            neighbors.push_back(std::pair<int,int>{currX-1, currY});
        }
        if (currX + 1 >= 0 && currY && visited[currX+1][currY]) { // right
            neighbors.push_back(std::pair<int,int>{currX+1, currY});
        }
        if (currX >= 0 && currY - 1 && visited[currX][currY-1]) { // up
            neighbors.push_back(std::pair<int,int>{currX, currY-1});
        }
        if (currX >= 0 && currY + 1 && visited[currX][currY+1]) { // down
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
