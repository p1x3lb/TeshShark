using System.Collections.Generic;

namespace Project.Scripts.Core.Field
{
    public class BfsShortestPath
    {
        private int rows;
        private int cols;
        private CellView[,] grid;

        public BfsShortestPath(CellView[,] grid)
        {
            this.grid = grid;
            rows = grid.GetLength(0);
            cols = grid.GetLength(1);
        }

        private bool IsValid(int row, int col)
        {
            return row >= 0 && row < rows && col >= 0 && col < cols;
        }

        private bool IsWalkable(int row, int col)
        {
            return IsValid(row, col) && grid[row, col].Walkable;
        }

        public List<(int, int)> FindShortestPath(int startRow, int startCol, int targetRow, int targetCol)
        {
            List<(int, int)> path = new List<(int, int)>();

            int[] dr = { -1, 0, 1, 0 };
            int[] dc = { 0, 1, 0, -1 };

            Queue<(int, int)> queue = new Queue<(int, int)>();
            queue.Enqueue((startRow, startCol));

            bool[,] visited = new bool[rows, cols];
            (int, int)[,] prevVertex = new (int, int)[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    visited[i, j] = false;
                    prevVertex[i, j] = (-1, -1);
                }
            }

            visited[startRow, startCol] = true;

            while (queue.Count > 0)
            {
                var (row, col) = queue.Dequeue();

                if (row == targetRow && col == targetCol)
                {
                    // Путь найден, восстанавливаем его
                    while (row != -1 && col != -1)
                    {
                        path.Add((row, col));
                        var (prevRow, prevCol) = prevVertex[row, col];
                        row = prevRow;
                        col = prevCol;
                    }

                    path.Reverse();
                    return path;
                }

                for (int i = 0; i < 4; i++)
                {
                    int newRow = row + dr[i];
                    int newCol = col + dc[i];

                    if (IsValid(newRow, newCol) && !visited[newRow, newCol] && IsWalkable(newRow, newCol))
                    {
                        queue.Enqueue((newRow, newCol));
                        visited[newRow, newCol] = true;
                        prevVertex[newRow, newCol] = (row, col);
                    }
                }
            }

            // Если путь не найден, возвращаем пустой список
            return path;
        }
    }
}