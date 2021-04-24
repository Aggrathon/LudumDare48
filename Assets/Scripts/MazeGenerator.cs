using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{

    int[,] maze;
    int num_sets;
    Stack<(int, int)> stack;

    public enum CellShape
    {
        Solid = 0,
        WallLeft = 1,
        WallRight = 2,
        WallLeftRight = 3,
        WallTop = 4,
        WallLeftTop = 5,
        WallRightTop = 6,
        WallLeftRightTop = 7,
        WallBottom = 8,
        WallLeftBottom = 9,
        WallRightBottom = 10,
        WallLeftRightBottom = 11,
        WallTopBottom = 12,
        WallLeftTopBottom = 13,
        WallRightTopBottom = 14,
        WallLeftRightTopBottom = 15,
        Space = 16,
        SpaceLeft = 16 + 1,
        SpaceRight = 16 + 2,
        SpaceLeftRight = 16 + 3,
        SpaceTop = 16 + 4,
        SpaceLeftTop = 16 + 5,
        SpaceRightTop = 16 + 6,
        SpaceLeftRightTop = 16 + 7,
        SpaceBottom = 16 + 8,
        SpaceLeftBottom = 16 + 9,
        SpaceRightBottom = 16 + 10,
        SpaceLeftRightBottom = 16 + 11,
        SpaceTopBottom = 16 + 12,
        SpaceLeftTopBottom = 16 + 13,
        SpaceRightTopBottom = 16 + 14,
        SpaceLeftRightTopBottom = 16 + 15,
    }

    public MazeGenerator(int width, int height)
    {
        maze = new int[width, height];
        stack = new Stack<(int, int)>();
        num_sets = 1;
    }

    public void OpenSpace(int x, int y)
    {
        FloodFill((x, y), num_sets);
        num_sets++;
    }

    public void OpenSpace(int x1, int x2, int y1, int y2)
    {
        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                stack.Push((x, y));
            }
        }
        FloodFill(stack.Pop(), num_sets);
        num_sets++;
    }

    public void Generate()
    {
        // This is a slow and naive implementation of Kruskals algorithm
        // TODO: Replace the FloodFill with a Union-Set structure
        List<(int, int)> unvisited = new List<(int, int)>(maze.Length / 4);
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                if ((x % 2 == 0) && (y % 2 == 0))
                    OpenSpace(x, y);
                else if ((x % 2 == 0) || (y % 2 == 0))
                    unvisited.Add((x, y));
            }
        }
        unvisited.Shuffle();
        foreach ((int x, int y) in unvisited)
        {
            KruskalCarve(x, y);
        }
    }

    public CellShape GetCell(int x, int y)
    {
        int cell = 0;
        if (maze[x, y] != 0)
        {
            cell = 16;
        }
        if (GetLeft(x, y).Item3 != 0)
        {
            cell += 1;
        }
        if (GetRight(x, y).Item3 != 0)
        {
            cell += 2;
        }
        if (GetUp(x, y).Item3 != 0)
        {
            cell += 4;
        }
        if (GetDown(x, y).Item3 != 0)
        {
            cell += 8;
        }
        return (CellShape)cell;
    }

    public IEnumerable<(int, int, CellShape)> GetCells()
    {
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                yield return (x, y, GetCell(x, y));
            }
        }
    }

    void KruskalCarve(int x, int y)
    {
        if (maze[x, y] != 0)
        {
            return;
        }
        int left = GetLeft(x, y).Item3;
        int right = GetRight(x, y).Item3;
        int up = GetUp(x, y).Item3;
        int down = GetDown(x, y).Item3;
        if (left != 0)
        {
            if (left == right)
                return;
            if (left == up)
                return;
            if (left == down)
                return;
        }
        if (right != 0)
        {
            if (right == up)
                return;
            if (right == down)
                return;
        }
        if (up != 0 & up == down)
            return;
        FloodFill((x, y), num_sets);
        num_sets++;
    }


    void FloodFill((int, int) pos, int set)
    {
        stack.Push(pos);
        while (stack.Count > 0)
        {
            (int x, int y) = stack.Pop();
            maze[x, y] = set;
            foreach ((int x2, int y2, int m) in GetNeighbours(x, y))
            {
                if (m == 0 | m == set)
                    continue;
                if (m < set)
                {
                    set = m;
                    stack.Push((x, y));
                    break;
                }
                else
                {
                    stack.Push((x2, y2));
                }
            }
        }
    }



    (int, int, int) GetLeft(int x, int y)
    {
        if (x == 0)
            return (0, y, 0);
        return (x - 1, y, maze[x - 1, y]);
    }
    (int, int, int) GetRight(int x, int y)
    {
        if (maze.GetLength(0) == x + 1)
            return (x, y, 0);
        return (x + 1, y, maze[x + 1, y]);
    }
    (int, int, int) GetUp(int x, int y)
    {
        if (y == 0)
            return (x, 0, 0);
        return (x, y - 1, maze[x, y - 1]);
    }
    (int, int, int) GetDown(int x, int y)
    {
        if (maze.GetLength(1) == y + 1)
            return (x, y, 0);
        return (x, y + 1, maze[x, y + 1]);
    }
    IEnumerable<(int, int, int)> GetNeighbours(int x, int y)
    {
        yield return GetLeft(x, y);
        yield return GetRight(x, y);
        yield return GetUp(x, y);
        yield return GetDown(x, y);
    }
}
