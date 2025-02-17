using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

public class MazeGeneration
{
    // Maze size
    public static int width = 10;
    public static int height = 10;
    static string type = "";

    // Cell object
    public struct Cell
    {
        public int x;
        public int y;
    }

    static readonly int UP = 1;
    static readonly int DOWN = 2;
    static readonly int RIGHT = 4;
    static readonly int LEFT = 8;
    static List<int> directions = new List<int>{ UP, DOWN, RIGHT, LEFT };

    
    

    // Initialise list of active cells (currently empty)
    public static List<Cell> cells = new List<Cell>();

    public static void Main(string[] args)
    {
        if (args.Length > 0) 
        {
            Int32.TryParse(args[0], out width);
        }
        if (args.Length > 1)
        {
            Int32.TryParse(args[1], out height);
        }
        if (args.Length > 2)
        {
            type = args[2];
        }
        
        int[,] grid = new int[height,width];
        

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = 0;
            }
        }

        Console.Write("\u001b[2J");

        PrintMaze(grid);
        Thread.Sleep(20);

        Random random = new Random();

        int x = random.Next(width);
        int y = random.Next(height);

        cells.Add(CreateCell(x, y));

        while (cells.Count > 0)
        {
            int chosenCell = ChooseCell(cells.Count, type);
            x = cells[chosenCell].x;
            y = cells[chosenCell].y;

            
            directions = directions.OrderBy(z => random.Next()).ToList();
            foreach (int i in directions) {
                int newX = x + GetXDirection(i);
                int newY = y + GetYDirection(i);

                if (newX >= 0 && newY >= 0 && newX < width && newY < height && grid[newY,newX] == 0) {
                    grid[y,x] |= i;
                    grid[newY, newX] |= OppositeDirection(i);
                    cells.Add(CreateCell(newX, newY));
                    chosenCell = -10;

                    PrintMaze(grid);
                    Thread.Sleep(20);
                    break;
                }
            }
            
            if (chosenCell >= 0 && chosenCell < cells.Count)
            {
                cells.RemoveAt(chosenCell);
            }
        }
        PrintFinalMaze(grid);
        Console.WriteLine();
    }

    // Prints the final version of the maze
    static void PrintFinalMaze(int[,] grid)
    {
        Random random = new Random();

        int doorDir = random.Next(2);
        int entranceCell = random.Next(1, grid.GetLength(doorDir) - 1);
        int exitCell = random.Next(1, grid.GetLength(doorDir) - 1);

        Console.Write("\u001b[H");

        if (doorDir == 0) {
            Console.WriteLine(" " + new string('_', grid.GetLength(1) * 2 - 1));
        }
        else {
            Console.WriteLine(" " + new string('_', entranceCell * 2 - 1) + "  " + new string('_', (grid.GetLength(1) - entranceCell) * 2 - 2));
        }
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            if (doorDir == 0 && entranceCell == y)
            {
                Console.Write(" ");
            }
            else {
                Console.Write("|");
            }
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                int cell = grid[y, x];

                if (cell == 0 && y + 1 < grid.GetLength(0) && grid[y + 1, x] == 0)
                {
                    Console.Write(" ");
                }
                else if (doorDir == 1 && exitCell == x && y == grid.GetLength(0) - 1)
                {
                    Console.WriteLine("  ");
                    break;
                }
                else
                {
                    Console.Write((cell & DOWN) != 0 ? " " : "_");
                }

                if (cell == 0 && x + 1 < grid.GetLength(1) && grid[y, x + 1] == 0)
                {
                    Console.Write((y + 1 < grid.GetLength(0) && (grid[y + 1, x] == 0 || grid[y + 1, x + 1] == 0)) ? " " : "_");
                }
                else if ((cell & RIGHT) != 0)
                {
                    Console.Write(((cell | grid[y, x + 1]) & DOWN) != 0 ? " " : "_");
                }
                else if (doorDir == 0 && exitCell == y && x == grid.GetLength(1) - 1)
                {
                    Console.Write(" ");
                }
                else {
                    Console.Write("|");
                }
            }
            Console.WriteLine();
        }
        
    }

    // Visually update the progress of the maze
    static void PrintMaze(int[,] grid) {
        Console.Write("\u001b[H");
        Console.WriteLine(" " + new string('_', grid.GetLength(1) * 2 - 1));

        for (int y = 0; y < grid.GetLength(0); y++)
        {
            Console.Write("|");
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                int cell = grid[y, x];

                if (cell == 0 && y + 1 < grid.GetLength(0) && grid[y + 1, x] == 0)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write((cell & DOWN) != 0 ? " " : "_");
                }

                if (cell == 0 && x + 1 < grid.GetLength(1) && grid[y, x + 1] == 0)
                {
                    Console.Write((y + 1 < grid.GetLength(0) && (grid[y + 1, x] == 0 || grid[y + 1, x + 1] == 0)) ? " " : "_");
                }
                else if ((cell & RIGHT) != 0)
                {
                    Console.Write(((cell | grid[y, x + 1]) & DOWN) != 0 ? " " : "_");
                }
                else
                {
                    Console.Write("|");
                }
            }
            Console.WriteLine();
        }
        
    }

    // Method for choosing what cell is picked next
    static int ChooseCell(int cellLength, string type)
    {
        if (type == "random")
        {
            Random random = new Random();
            return random.Next(cellLength);
        }
        if (type == "oldest") {
            return 0;
        }
        if (type == "weird") {
            if (cellLength % 2 == 0) {
                return cellLength - 1;
            }
            else {
                return 0;
            }
        }
        if (type == "two") {
            if (cellLength > 1) {
                return cellLength - 2 ;
            }
            else return 0;
        }
        if (type == "newest") {
            return cellLength -1;
        }
        else {
            return cellLength - 1;
        }

    }
    
    // Create a new cell
    static Cell CreateCell(int a, int b) {
        Cell cell = new Cell()
        {
            x = a,
            y = b
        };
        return cell;
        
    }

    // Get horizontal direction
    static int GetXDirection(int direction) {
        int newX = 0;
        if (direction == UP || direction == DOWN) {
            newX = 0;
        }
        if (direction == RIGHT) {
            newX = 1;
        }
        if (direction == LEFT) {
            newX = -1;
        }
        return newX;
    }

    // Gets vertically direction
    static int GetYDirection(int direction) {
        int newY = 0;
        if (direction == RIGHT || direction == LEFT) {
            newY = 0;
        }
        if (direction == UP) {
            newY = -1;
        }
        if (direction == DOWN) {
            newY = 1;
        }
        return newY;
    }

    // Get the opposite direction of the current
    static int OppositeDirection(int direction) {
        int newDirection = 0;
        if (direction == UP) {
            newDirection = DOWN;
        }
        if (direction == DOWN) {
            newDirection = UP;
        }
        if (direction == LEFT) {
            newDirection = RIGHT;
        }
        if (direction == RIGHT) {
            newDirection = LEFT;
        }
        return newDirection;
    }


}