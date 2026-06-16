using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.GameOfLife;

public class GameOfLifeArte : IArte
{
    public string Nome => "Game of Life";
    public string Descricao => "Autômato celular de Conway";

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth - 1;
        int altura = Console.WindowHeight - 2;
        var grid = new bool[altura, largura];
        var random = new Random();

        // Seed aleatório ~30%
        for (int r = 0; r < altura; r++)
            for (int c = 0; c < largura; c++)
                grid[r, c] = random.Next(100) < 30;

        Console.CursorVisible = false;
        int gen = 0;

        while (!ct.IsCancellationRequested)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"  Game of Life — Geração: {gen++}");
            Console.ResetColor();

            for (int r = 0; r < altura; r++)
            {
                Console.SetCursorPosition(0, r + 1);
                for (int c = 0; c < largura; c++)
                {
                    if (grid[r, c])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('█');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }

            Console.ResetColor();
            grid = ProximaGeracao(grid, altura, largura);
            Thread.Sleep(100);
        }
    }

    private static bool[,] ProximaGeracao(bool[,] grid, int rows, int cols)
    {
        var next = new bool[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                int vizinhos = ContarVizinhos(grid, r, c, rows, cols);
                next[r, c] = grid[r, c]
                    ? vizinhos is 2 or 3
                    : vizinhos == 3;
            }
        return next;
    }

    private static int ContarVizinhos(bool[,] grid, int r, int c, int rows, int cols)
    {
        int count = 0;
        for (int dr = -1; dr <= 1; dr++)
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;
                int nr = (r + dr + rows) % rows;
                int nc = (c + dc + cols) % cols;
                if (grid[nr, nc]) count++;
            }
        return count;
    }
}
