using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Labirinto;

public class LabirintoArte : IArte
{
    public string Nome => "Labirinto";
    public string Descricao => "Geração procedural + resolução animada (DFS)";

    public void Executar(CancellationToken ct)
    {
        Console.CursorVisible = false;

        while (!ct.IsCancellationRequested)
        {
            int largura = Console.WindowWidth;
            int altura = Console.WindowHeight - 1;
            int mazeW = (largura - 1) / 2;
            int mazeH = (altura - 1) / 2;
            if (mazeW < 3) mazeW = 3;
            if (mazeH < 3) mazeH = 3;

            var grid = new bool[mazeH * 2 + 1, mazeW * 2 + 1]; // true = caminho
            var rng = new Random();

            Console.Clear();

            // Gerar labirinto (DFS animado)
            GerarLabirinto(grid, mazeW, mazeH, rng, ct);
            if (ct.IsCancellationRequested) break;

            // Marcar entrada e saída
            grid[1, 0] = true;
            grid[mazeH * 2 - 1, mazeW * 2] = true;

            DesenharCompleto(grid);
            Thread.Sleep(500);
            if (ct.IsCancellationRequested) break;

            // Resolver (BFS animado)
            ResolverLabirinto(grid, ct);
            if (ct.IsCancellationRequested) break;

            // Esperar antes de gerar novo
            Thread.Sleep(2000);
        }

        Console.ResetColor();
    }

    private static void GerarLabirinto(bool[,] grid, int mazeW, int mazeH, Random rng, CancellationToken ct)
    {
        var visited = new bool[mazeH, mazeW];
        var stack = new Stack<(int X, int Y)>();
        stack.Push((0, 0));
        visited[0, 0] = true;
        grid[1, 1] = true;

        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };
        int animCounter = 0;

        while (stack.Count > 0 && !ct.IsCancellationRequested)
        {
            var (cx, cy) = stack.Peek();
            var neighbors = new List<int>();

            for (int d = 0; d < 4; d++)
            {
                int nx = cx + dx[d], ny = cy + dy[d];
                if (nx >= 0 && nx < mazeW && ny >= 0 && ny < mazeH && !visited[ny, nx])
                    neighbors.Add(d);
            }

            if (neighbors.Count > 0)
            {
                int dir = neighbors[rng.Next(neighbors.Count)];
                int nx = cx + dx[dir], ny = cy + dy[dir];
                visited[ny, nx] = true;

                // Abrir parede entre células
                int wx = 1 + cx * 2 + dx[dir];
                int wy = 1 + cy * 2 + dy[dir];
                grid[wy, wx] = true;
                grid[1 + ny * 2, 1 + nx * 2] = true;

                // Animação
                animCounter++;
                if (animCounter % 3 == 0)
                {
                    int cols = grid.GetLength(1);
                    if (wx < cols && wy < grid.GetLength(0) && wx >= 0 && wy >= 0)
                    {
                        Console.SetCursorPosition(wx, wy);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(' ');
                    }
                    int cellX = 1 + nx * 2, cellY = 1 + ny * 2;
                    if (cellX < cols && cellY < grid.GetLength(0))
                    {
                        Console.SetCursorPosition(cellX, cellY);
                        Console.Write(' ');
                    }
                    Thread.Sleep(5);
                }

                stack.Push((nx, ny));
            }
            else
            {
                stack.Pop();
            }
        }
    }

    private static void DesenharCompleto(bool[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (grid[y, x])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(' ');
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write('█');
                }
            }
            if (y < rows - 1) Console.WriteLine();
        }
    }

    private static void ResolverLabirinto(bool[,] grid, CancellationToken ct)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        var start = (X: 0, Y: 1);
        var end = (X: cols - 1, Y: rows - 2);

        var visited = new bool[rows, cols];
        var parent = new (int X, int Y)?[rows, cols];
        var queue = new Queue<(int X, int Y)>();

        queue.Enqueue(start);
        visited[start.Y, start.X] = true;

        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };
        int animCount = 0;

        while (queue.Count > 0 && !ct.IsCancellationRequested)
        {
            var (cx, cy) = queue.Dequeue();

            if (cx == end.X && cy == end.Y)
            {
                // Traçar caminho
                var pos = (end.X, end.Y);
                while (parent[pos.Item2, pos.Item1] != null)
                {
                    Console.SetCursorPosition(pos.Item1, pos.Item2);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('●');
                    pos = parent[pos.Item2, pos.Item1]!.Value;
                    Thread.Sleep(15);
                    if (ct.IsCancellationRequested) return;
                }
                return;
            }

            for (int d = 0; d < 4; d++)
            {
                int nx = cx + dx[d], ny = cy + dy[d];
                if (nx >= 0 && nx < cols && ny >= 0 && ny < rows && !visited[ny, nx] && grid[ny, nx])
                {
                    visited[ny, nx] = true;
                    parent[ny, nx] = (cx, cy);
                    queue.Enqueue((nx, ny));

                    animCount++;
                    if (animCount % 5 == 0)
                    {
                        Console.SetCursorPosition(nx, ny);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write('·');
                        Thread.Sleep(2);
                    }
                }
            }
        }
    }
}
